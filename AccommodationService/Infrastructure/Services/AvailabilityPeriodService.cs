using AccommodationService.Application.Repositories;
using AccommodationService.Application.Services;
using AccommodationService.Domain;

namespace AccommodationService.Infrastructure.Services;

public class AvailabilityPeriodService : IAvailabilityPeriodService
{
    private readonly IAvailabilityPeriodRepository availabilityPeriodRepository;

    public AvailabilityPeriodService(IAvailabilityPeriodRepository availabilityPeriodRepository)
    {
        this.availabilityPeriodRepository = availabilityPeriodRepository;
    }

    public async Task<AvailabilityPeriod> CreateAsync(AvailabilityPeriod availabilityPeriod)
    {
        var overlappingPeriods = await availabilityPeriodRepository
            .GetOverlappingPeriodsAsync(availabilityPeriod.PropertyId, availabilityPeriod.StartDate, availabilityPeriod.EndDate);

        if (overlappingPeriods.Any())
        {
            throw new Exception("The new availability period overlaps with an existing one.");
        }
        var createdAvailabilityPeriod = await availabilityPeriodRepository.AddAsync(availabilityPeriod);
        return createdAvailabilityPeriod;
    }

    public async Task DeleteAsync(Guid id)
    {
        var availabilityPeriod = await availabilityPeriodRepository.GetAsync(id);
        availabilityPeriodRepository.Delete(availabilityPeriod);
    }

    public async Task<IEnumerable<AvailabilityPeriod>> GetAllByPropertyIdAsync(Guid propertyId)
    {
        var availabilityPeriods = await availabilityPeriodRepository.GetAllByPropertyIdAsync(propertyId);
        return availabilityPeriods;
    }

    public async Task<AvailabilityPeriod> GetAsync(Guid id)
    {
        return await availabilityPeriodRepository.GetAsync(id);
    }

    public async Task<AvailabilityPeriod> UpdateAsync(AvailabilityPeriod availabilityPeriod)
    {
        var updatedAvailabilityPeriod = availabilityPeriodRepository.Update(availabilityPeriod);
        return await Task.FromResult(updatedAvailabilityPeriod);
    }

}
