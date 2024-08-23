using AccommodationService.Domain;

namespace AccommodationService.Application.Services;

public interface IAvailabilityPeriodService
{
    Task<IEnumerable<AvailabilityPeriod>> GetAllByPropertyIdAsync(Guid propertyId);
    Task<AvailabilityPeriod> GetAsync(Guid id);
    Task<AvailabilityPeriod> CreateAsync(AvailabilityPeriod availabilityPeriod);
    Task<AvailabilityPeriod> UpdateAsync(AvailabilityPeriod availabilityPeriod);
    Task DeleteAsync(Guid id);
}
