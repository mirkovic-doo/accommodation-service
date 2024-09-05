using AccommodationService.Domain;

namespace AccommodationService.Application.Repositories;

public interface IPropertyRepository : IBaseRepository<Property>
{
    Task<IEnumerable<Property>> GetMyAsync();
    Task<Property> GetMyByIdAsync(Guid id);
    Task<IEnumerable<Property>> SearchPropertiesAsync(string location, int guests, DateOnly startDate, DateOnly endDate);
}
