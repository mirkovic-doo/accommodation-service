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

    public async Task<Property> GetAsync(Guid id)
    {
        return await propertyRepository.GetAsync(id);
    }

    public async Task Delete(Guid id)
    {
        var property = await propertyRepository.GetAsync(id);
        propertyRepository.Delete(property);
    }

    public async Task<Property> Update(Property property)
    {
        var updatedProperty = propertyRepository.Update(property);
        return await Task.FromResult(updatedProperty);
    }
}
