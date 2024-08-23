using AccommodationService.Application.Repositories;
using AccommodationService.Domain;

namespace AccommodationService.Infrastructure.Repositories;

public class PropertyRepository : BaseRepository<Property>, IBaseRepository<Property>, IPropertyRepository
{
    private readonly AccommodationDbContext dbContext;

    public PropertyRepository(AccommodationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }
}
