using AccommodationService.Application.Repositories;
using AccommodationService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Infrastructure.Repositories;

public class PropertyRepository : BaseRepository<Property>, IBaseRepository<Property>, IPropertyRepository
{
    private readonly AccommodationDbContext dbContext;

    public PropertyRepository(AccommodationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Property>> SearchPropertiesAsync(string location, int guests, DateTime startDate, DateTime endDate)
    {
        return await dbContext.Set<Property>()
        .Include(p => p.AvailabilityPeriods)
        .Where(p => p.Location.ToLower().Contains(location.ToLower()) &&
                    p.MinGuests <= guests &&
                    p.MaxGuests >= guests &&
                    p.AvailabilityPeriods.Any(
                        ap =>
                        ap.StartDate.ToUniversalTime() <= startDate.ToUniversalTime() &&
                        ap.EndDate.ToUniversalTime() >= endDate.ToUniversalTime())
                    )
        .ToListAsync();
    }
}
