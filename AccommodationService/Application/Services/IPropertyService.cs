using AccommodationService.Controllers.Property.Requests;
using AccommodationService.Controllers.Property.Responses;
using AccommodationService.Domain;

namespace AccommodationService.Application.Services;

public interface IPropertyService
{
    Task<Property> CreateAsync(Property property);
    Task<Property> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<Property> UpdateAsync(PropertyRequest request);
    Task<IEnumerable<SearchPropertyResponse>> SearchPropertiesAsync(string location, int guests, DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<Property>> GetMyAsync();
    Task DeletePropertiesAsync();
}
