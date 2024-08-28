using AccommodationService.Domain;

namespace AccommodationService.Application.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetAllByPropertyIdAsync(Guid propertyId);

    Task<IEnumerable<Reservation>> GetAllPendingCorrelated(Reservation reservation);

    Task<int> GetNumberOfCancelledReservationsAsync(Guid guestId);

    Task<IEnumerable<Reservation>> GetGuestReservationsAsync(Guid guestId);
    Task DeleteGuestReservationsAsync(Guid guestId);
}
