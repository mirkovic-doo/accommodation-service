using AccommodationService.Domain;

namespace AccommodationService.Application.Services;

public interface IReservationService
{
    Task<Reservation> CreateAsync(Reservation reservation);

    Task<Reservation> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<IEnumerable<Reservation>> GetAllByPropertyIdAsync(Guid propertyId);

    Task CancelReservationAsync(Guid id);
}
