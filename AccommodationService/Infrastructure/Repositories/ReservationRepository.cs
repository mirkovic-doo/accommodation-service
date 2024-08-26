using AccommodationService.Application.Repositories;
using AccommodationService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Infrastructure.Repositories;

public class ReservationRepository : BaseRepository<Reservation>, IBaseRepository<Reservation>, IReservationRepository
{
    private readonly AccommodationDbContext dbContext;

    public ReservationRepository(AccommodationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Reservation>> GetAllByPropertyIdAsync(Guid propertyId)
    {
        return await dbContext.Reservations.Where(ap => ap.PropertyId == propertyId).ToListAsync();
    }
}
