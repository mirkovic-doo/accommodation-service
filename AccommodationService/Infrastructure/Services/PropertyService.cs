using AccommodationService.Application.Repositories;
using AccommodationService.Application.Services;
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

    public async Task<Property> UpdateAsync(Property property)
    {
        var updatedProperty = propertyRepository.Update(property);
        return await Task.FromResult(updatedProperty);
    }

    public async Task<IEnumerable<SearchPropertyResponse>> SearchPropertiesAsync(
        string location, int guests, string startDate, string endDate)
    {
        if (!DateOnly.TryParse(startDate, out var parsedStartDate) ||
        !DateOnly.TryParse(endDate, out var parsedEndDate))
        {
            throw new Exception("Invalid date format.");
        }

        var properties = await propertyRepository.SearchPropertiesAsync(location, guests, parsedStartDate, parsedEndDate);
        return CalculatePrices(properties, guests, parsedStartDate, parsedEndDate);
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

                if (applicablePeriod != null)
                {
                    totalPrice += applicablePeriod.PricePerDay;
                }
                else
                {
                    // If any day within the range is not available, skip this property.
                    totalPrice = 0;
                    break;
                }
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
}
