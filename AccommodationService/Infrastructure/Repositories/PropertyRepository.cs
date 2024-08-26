using AccommodationService.Application.Repositories;
using AccommodationService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Infrastructure.Repositories;

public class PropertyRepository : BaseRepository<Property>, IBaseRepository<Property>, IPropertyRepository
{
    public PropertyRepository(AccommodationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Property>> GetMyAsync()
    {
        return await dbContext.Properties.Where(p => p.CreatedById == dbContext.CurrentUserId).ToListAsync();
    }

    public async Task<Property> GetMyByIdAsync(Guid id)
    {
        var property = await dbContext.Properties.SingleOrDefaultAsync(p => p.Id == id && p.CreatedById == dbContext.CurrentUserId);

        if (property == null)
        {
            throw new Exception("Property not found");
        }

        return property;
    }

    public async Task<IEnumerable<Property>> SearchPropertiesAsync(string location, int guests, DateOnly startDate, DateOnly endDate)
    {
        return await dbContext.Set<Property>()
        .Include(p => p.AvailabilityPeriods)
        .Where(p => p.Location.ToLower().Contains(location.ToLower()) &&
                    p.MinGuests <= guests &&
                    p.MaxGuests >= guests &&
                    p.AvailabilityPeriods.Any(
                        ap =>
                        (ap.StartDate <= startDate && ap.EndDate >= startDate) ||
                        (ap.StartDate <= endDate && ap.EndDate >= endDate)
                    ))
        .ToListAsync();
    }
}
