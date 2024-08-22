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
        return await dbContext.Set<AvailabilityPeriod>().Where(ap => ap.PropertyId == propertyId).ToListAsync();
    }

    public async Task<AvailabilityPeriod?> FindClosestStartDateAsync(Guid propertyId, DateTime startDate)
    {
        return await dbContext.Set<AvailabilityPeriod>()
            .Where(ap => ap.PropertyId == propertyId && ap.StartDate <= startDate)
            .OrderByDescending(ap => ap.StartDate)
            .FirstOrDefaultAsync();
    }
}
