using AccommodationService.Domain;

namespace AccommodationService.Application.Repositories;

public interface IAvailabilityPeriodRepository : IBaseRepository<AvailabilityPeriod>
{
    Task<IEnumerable<AvailabilityPeriod>> GetAllByPropertyIdAsync(Guid propertyId);
    Task<IEnumerable<AvailabilityPeriod>> GetOverlappingPeriodsAsync(Guid propertyId, DateOnly startDate, DateOnly endDate);
}
