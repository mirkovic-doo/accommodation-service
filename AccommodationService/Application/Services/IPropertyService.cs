using AccommodationService.Domain;

namespace AccommodationService.Application.Services;

public interface IPropertyService
{
    Task<Property> CreateAsync(Property property);

    Task<Property> GetAsync(Guid id);

    void Delete(Guid id);

    Property Update(Property property);
}
