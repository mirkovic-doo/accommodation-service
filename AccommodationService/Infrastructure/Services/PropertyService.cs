using AccommodationService.Application.Repositories;
using AccommodationService.Application.Services;
using AccommodationService.Domain;

namespace AccommodationService.Infrastructure.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository propertyRepository;

    public PropertyService(IPropertyRepository propertyRepository)
    {
        this.propertyRepository = propertyRepository;
    }

    public async Task<Property> CreateAsync(Property property)
    {
        var createdProperty = await propertyRepository.AddAsync(property);

        return createdProperty;
    }
}
