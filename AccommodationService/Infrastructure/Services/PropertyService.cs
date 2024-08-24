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
            var relevantPeriods = property.AvailabilityPeriods
            .Where(ap => ap.StartDate <= startDate && ap.EndDate >= endDate)
            .ToList();


            if (relevantPeriods.Count == 0) continue;

            var totalPrice = relevantPeriods.Sum(ap => ap.PricePerDay * (endDate.DayNumber - startDate.DayNumber));
            var unitPrice = property.PricingOption == PricingOption.PerGuest
                            ? totalPrice / guests
                            : totalPrice / (endDate.DayNumber - startDate.DayNumber);

            var searchPropertyResponse = mapper.Map<SearchPropertyResponse>(property);
            searchPropertyResponse.UnitPrice = Math.Round(unitPrice, 2);
            searchPropertyResponse.TotalPrice = Math.Round(totalPrice, 2);

            searchPropertyResponses.Add(searchPropertyResponse);
        }
        return searchPropertyResponses;
    }
}
