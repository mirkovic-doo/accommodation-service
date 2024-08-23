using AccommodationService.Domain;

namespace AccommodationService.Application.Repositories;

public interface IPropertyRepository : IBaseRepository<Property>
{
    Task<IEnumerable<Property>> SearchPropertiesAsync(string location, int guests, DateTime startDate, DateTime endDate);
}
