using AccommodationService.Application.Repositories;
using AccommodationService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Infrastructure.Repositories;

public class AvailabilityPeriodRepository : BaseRepository<AvailabilityPeriod>, IBaseRepository<AvailabilityPeriod>, IAvailabilityPeriodRepository
{
    private readonly AccommodationDbContext dbContext;

    public AvailabilityPeriodRepository(AccommodationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<AvailabilityPeriod>> GetAllByPropertyIdAsync(Guid propertyId)
    {
        return await dbContext.AvailabilityPeriods.Where(ap => ap.PropertyId == propertyId).ToListAsync();
    }

    public async Task<IEnumerable<AvailabilityPeriod>> GetOverlappingPeriodsAsync(Guid propertyId, DateOnly startDate, DateOnly endDate)
    {
        return await dbContext.AvailabilityPeriods
        .Where(ap => ap.PropertyId == propertyId &&
            (
                (startDate >= ap.StartDate && endDate <= ap.EndDate) ||  // Completely inside
                (startDate >= ap.StartDate && startDate <= ap.EndDate) ||  // Starts inside
                (endDate >= ap.StartDate && endDate <= ap.EndDate) ||  // Ends inside
                (startDate <= ap.StartDate && endDate >= ap.EndDate)  // Completely hugs
            )
        )
        .ToListAsync();
    }
}
