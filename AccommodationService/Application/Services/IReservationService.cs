using AccommodationService.Domain;

namespace AccommodationService.Application.Services;

public interface IReservationService
{
    Task<Reservation> CreateAsync(Reservation reservation);

    Task<Reservation> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<IEnumerable<Reservation>> GetAllByPropertyIdAsync(Guid propertyId);

    Task ConfirmReservationAsync(Guid id);

    Task CancelReservationGuestAsync(Guid id);

    Task CancelReservationHostAsync(Guid id);

    Task<int> GetNumberOfCancelledReservationsAsync(Guid guestId);

    Task<IEnumerable<Reservation>> GetGuestReservationsAsync(Guid guestId);

}
