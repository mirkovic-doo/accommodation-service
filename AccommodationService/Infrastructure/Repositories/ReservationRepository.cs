using AccommodationService.Application.Repositories;
using AccommodationService.Domain;
using AccommodationService.Domain.Enums;
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
        return await dbContext.Reservations.Where(r => r.PropertyId == propertyId).ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllPendingCorrelated(Reservation reservation)
    {
        return await dbContext.Reservations
            .Where(r => r.Status == ReservationStatus.Pending &&
            (r.StartDate <= reservation.StartDate && r.EndDate >= reservation.StartDate) ||
            (r.StartDate <= reservation.EndDate && r.EndDate >= reservation.EndDate))
            .ToListAsync();
    }

    public async Task<int> GetNumberOfCancelledReservationsAsync(Guid guestId)
    {
        return await dbContext.Reservations
            .Where(r => r.CreatedById == guestId && r.Status == ReservationStatus.GuestCancelled)
            .CountAsync();
    }
}
