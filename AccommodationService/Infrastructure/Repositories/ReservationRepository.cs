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

    public override async Task<Reservation> GetAsync(Guid id)
    {
        var entity = await dbContext.Reservations.Include(r => r.Property).SingleOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            throw new Exception($"Entity with {id} not found");
        }

        return entity;
    }

    public async Task DeleteGuestReservationsAsync(Guid guestId)
    {
        await dbContext.Reservations
            .Where(r => r.CreatedById == guestId)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllByPropertyIdAsync(Guid propertyId)
    {
        return await dbContext.Reservations
            .Include(r => r.Property)
            .Where(r => r.PropertyId == propertyId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllPendingCorrelated(Reservation reservation)
    {
        return await dbContext.Reservations
            .Include(r => r.Property)
            .Where(r => r.Status == ReservationStatus.Pending &&
            r.Id != reservation.Id &&
            r.PropertyId == reservation.PropertyId &&
            ((r.StartDate <= reservation.StartDate && r.EndDate >= reservation.StartDate) ||
            (r.StartDate <= reservation.EndDate && r.EndDate >= reservation.EndDate)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetGuestReservationsAsync(Guid guestId)
    {
        return await dbContext.Reservations
            .Include(r => r.Property)
            .Where(r => r.CreatedById == guestId)
            .ToListAsync();
    }

    public async Task<int> GetNumberOfCancelledReservationsAsync(Guid guestId)
    {
        return await dbContext.Reservations
            .Where(r => r.CreatedById == guestId && r.Status == ReservationStatus.GuestCancelled)
            .CountAsync();
    }

    public async Task<IEnumerable<Reservation>> GetHostReservationsAsync(Guid hostId)
    {
        return await dbContext.Reservations
            .Include(r => r.Property)
            .Where(r => r.Property.CreatedById == hostId)
            .ToListAsync();
    }
}
