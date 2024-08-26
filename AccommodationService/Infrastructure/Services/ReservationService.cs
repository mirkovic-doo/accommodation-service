using AccommodationService.Application.Repositories;
using AccommodationService.Application.Services;
using AccommodationService.Domain;

namespace AccommodationService.Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository reservationRepository;
    private readonly IPropertyRepository propertyRepository;

    public ReservationService(IReservationRepository reservationRepository, IPropertyRepository propertyRepository)
    {
        this.reservationRepository = reservationRepository;
        this.propertyRepository = propertyRepository;
    }

    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        decimal totalPrice = 0;
        var property = await propertyRepository.GetAsync(reservation.PropertyId);
        if (reservation.Guests < property.MinGuests || reservation.Guests > property.MaxGuests)
        {
            throw new Exception("Number of guests is either greater than maximum number of guests or lower than minimum number of guests");
        }
        for (var date = reservation.StartDate; date <= reservation.EndDate; date = date.AddDays(1))
        {
            var applicablePeriod = property.AvailabilityPeriods.FirstOrDefault(ap => ap.StartDate <= date && ap.EndDate >= date);
            var alreadyExists = property.Reservations.Any(r => r.StartDate <= date && r.EndDate >= date);
            if (applicablePeriod == null || alreadyExists)
            {
                throw new Exception("No available periods for this reservation");
            }
            totalPrice += applicablePeriod.PricePerDay;
        }
        reservation.Price = totalPrice;
        var createdReservation = await reservationRepository.AddAsync(reservation);
        return createdReservation;
    }

    public async Task DeleteAsync(Guid id)
    {
        var property = await reservationRepository.GetAsync(id);
        reservationRepository.Delete(property);
    }

    public async Task<IEnumerable<Reservation>> GetAllByPropertyIdAsync(Guid propertyId)
    {
        var reservations = await reservationRepository.GetAllByPropertyIdAsync(propertyId);
        return reservations;
    }

    public async Task<Reservation> GetAsync(Guid id)
    {
        return await reservationRepository.GetAsync(id);
    }
}
