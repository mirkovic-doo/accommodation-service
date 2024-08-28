using AccommodationService.Application.Repositories;
using AccommodationService.Application.Services;
using AccommodationService.Controllers.Property.Requests;
using AccommodationService.Controllers.Property.Responses;
using AccommodationService.Domain;
using AccommodationService.Domain.Enums;
using AutoMapper;

namespace AccommodationService.Infrastructure.Services;

public class PropertyService : IPropertyService
{
    private readonly IMapper mapper;
    private readonly IPropertyRepository propertyRepository;

    public PropertyService(IMapper mapper, IPropertyRepository propertyRepository)
    {
        this.mapper = mapper;
        this.propertyRepository = propertyRepository;
    }

    public async Task<Property> CreateAsync(Property property)
    {
        var createdProperty = await propertyRepository.AddAsync(property);
        return createdProperty;
    }

    public async Task<Property> GetAsync(Guid id)
    {
        return await propertyRepository.GetAsync(id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var property = await propertyRepository.GetAsync(id);
        propertyRepository.Delete(property);
    }

    public async Task<Property> UpdateAsync(PropertyRequest request)
    {
        if (request.Id == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var property = await propertyRepository.GetMyByIdAsync((Guid)request.Id);

        mapper.Map(request, property);

        return propertyRepository.Update(property);
    }

    public async Task<IEnumerable<SearchPropertyResponse>> SearchPropertiesAsync(
        string location, int guests, DateOnly startDate, DateOnly endDate)
    {
        var properties = await propertyRepository.SearchPropertiesAsync(location, guests, startDate, endDate);
        return CalculatePrices(properties, guests, startDate, endDate);
    }

    private IEnumerable<SearchPropertyResponse> CalculatePrices(
        IEnumerable<Property> properties, int guests, DateOnly startDate, DateOnly endDate)
    {
        var searchPropertyResponses = new List<SearchPropertyResponse>();
        foreach (var property in properties)
        {
            decimal totalPrice = 0;
            var days = endDate.DayNumber - startDate.DayNumber + 1;

            // TODO: Add logic when there are reservations in the period
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var applicablePeriod = property.AvailabilityPeriods.FirstOrDefault(ap => ap.StartDate <= date && ap.EndDate >= date);
                var reservationExists = property.Reservations.Any(r => r.StartDate <= date && r.EndDate >= date && r.Status == ReservationStatus.Confirmed);

                if (applicablePeriod == null || reservationExists)
                {
                    totalPrice = 0;
                    break;
                }
                totalPrice += applicablePeriod.PricePerDay;

            }

            if (totalPrice > 0)
            {
                var unitPrice = property.PricingOption == PricingOption.PerGuest
                                ? totalPrice / guests / days
                                : totalPrice / days;

                var searchPropertyResponse = mapper.Map<SearchPropertyResponse>(property);
                searchPropertyResponse.UnitPrice = Math.Round(unitPrice, 2);
                searchPropertyResponse.TotalPrice = Math.Round(totalPrice, 2);

                searchPropertyResponses.Add(searchPropertyResponse);
            }
        }
        return searchPropertyResponses;
    }

    public async Task<IEnumerable<Property>> GetMyAsync()
    {
        return await propertyRepository.GetMyAsync();
    }

    public async Task DeletePropertiesAsync()
    {
        var myProperties = await propertyRepository.GetMyAsync();

        var reservations = myProperties.SelectMany(p => p.Reservations).ToList();

        if (reservations.Any(r => r.Status == ReservationStatus.Confirmed && r.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow)))
        {
            throw new Exception("You can not delete properties with confirmed reservations");
        }

        foreach (var property in myProperties)
        {
            propertyRepository.Delete(property);
        }
    }
}
