using AccommodationService.Controllers.Property.Responses;
using AccommodationService.Domain;

namespace AccommodationService.Application.Services;

public interface IPropertyService
{
    Task<Property> CreateAsync(Property property);

    Task<Property> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<Property> UpdateAsync(Property property);

    Task<IEnumerable<SearchPropertyResponse>> SearchPropertiesAsync(string location, int guests, string startDate, string endDate);
}
