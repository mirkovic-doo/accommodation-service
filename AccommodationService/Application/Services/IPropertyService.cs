using AccommodationService.Domain;

namespace AccommodationService.Application.Services;

public interface IPropertyService
{
    Task<Property> CreateAsync(Property property);
}
