using AccommodationService.Application.Repositories;
using AccommodationService.Application.Services;
using AccommodationService.Domain;
using AccommodationService.Domain.Enums;
using AccommodationService.Notification;

namespace AccommodationService.Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IPropertyRepository propertyRepository;
    private readonly IReservationRepository reservationRepository;
    private readonly INotificationSenderService notificationSenderService;

    public ReservationService(
        IPropertyRepository propertyRepository,
        IReservationRepository reservationRepository,
        INotificationSenderService notificationSenderService)
    {
        this.propertyRepository = propertyRepository;
        this.reservationRepository = reservationRepository;
        this.notificationSenderService = notificationSenderService;
    }

    public async Task CancelReservationGuestAsync(Guid id)
    {
        var reservation = await reservationRepository.GetAsync(id);
        if (reservation.Status != ReservationStatus.Confirmed) throw new Exception("Reservation is not confirmed, therefore it can not be canceled");
        if (DateOnly.FromDateTime(DateTime.Now) >= reservation.StartDate) throw new Exception("Reservation already started");

        reservation.Status = ReservationStatus.GuestCancelled;
        reservationRepository.Update(reservation);

        notificationSenderService.Send(new NotificationPayload
        {
            EntityId = reservation.Id,
            ReceiverId = reservation.Property.CreatedById,
            SenderId = reservation.CreatedById,
            Type = NotificationType.ReservationResponse
        });
    }

    public async Task CancelReservationHostAsync(Guid id)
    {
        var reservation = await reservationRepository.GetAsync(id);
        if (reservation.Status != ReservationStatus.Pending) throw new Exception("Reservation is already confirmed or canceled");
        if (DateOnly.FromDateTime(DateTime.Now) >= reservation.StartDate) throw new Exception("Reservation already started");

        reservation.Status = ReservationStatus.HostCancelled;
        reservationRepository.Update(reservation);

        notificationSenderService.Send(new NotificationPayload
        {
            EntityId = reservation.Id,
            ReceiverId = reservation.CreatedById,
            SenderId = reservation.Property.CreatedById,
            Type = NotificationType.ReservationResponse
        });
    }

    public async Task ConfirmReservationAsync(Guid id)
    {
        var reservation = await reservationRepository.GetAsync(id);
        if (reservation.Status != ReservationStatus.Pending) throw new Exception("Reservation is already confirmed or canceled");
        if (DateOnly.FromDateTime(DateTime.Now) >= reservation.StartDate) throw new Exception("Reservation already started");

        var otherReservations = await reservationRepository.GetAllPendingCorrelated(reservation);
        foreach (var otherReservation in otherReservations)
        {
            otherReservation.Status = ReservationStatus.HostCancelled;
        }
        reservationRepository.UpdateRange(otherReservations);

        foreach (var cancelledReservation in otherReservations)
        {
            notificationSenderService.Send(new NotificationPayload
            {
                EntityId = cancelledReservation.Id,
                ReceiverId = cancelledReservation.CreatedById,
                SenderId = cancelledReservation.Property.CreatedById,
                Type = NotificationType.ReservationResponse
            });
        }

        reservation.Status = ReservationStatus.Confirmed;
        reservationRepository.Update(reservation);

        notificationSenderService.Send(new NotificationPayload
        {
            EntityId = reservation.Id,
            ReceiverId = reservation.CreatedById,
            SenderId = reservation.Property.CreatedById,
            Type = NotificationType.ReservationResponse
        });
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
            var alreadyExists = property.Reservations.Any(r => r.StartDate <= date && r.EndDate >= date && r.Status == ReservationStatus.Confirmed);

            if (applicablePeriod == null || alreadyExists)
            {
                throw new Exception("No available periods for this reservation");
            }
            totalPrice += applicablePeriod.PricePerDay;
        }

        reservation.Price = totalPrice;
        var createdReservation = await reservationRepository.AddAsync(reservation);

        notificationSenderService.Send(new NotificationPayload
        {
            EntityId = createdReservation.Id,
            ReceiverId = property.CreatedById,
            SenderId = createdReservation.CreatedById,
            Type = NotificationType.ReservationRequest
        });

        if (property.AutoConfirmReservation)
        {
            await ConfirmReservationAsync(createdReservation.Id);
        }
        return createdReservation;
    }

    public async Task DeleteAsync(Guid id)
    {
        var reservation = await reservationRepository.GetAsync(id);
        if (reservation.Status != ReservationStatus.Pending)
        {
            throw new Exception("You can only delete reservation request");
        }
        reservationRepository.Delete(reservation);
    }

    public async Task DeleteGuestReservationsAsync(Guid guestId)
    {
        var guestReservations = await reservationRepository.GetGuestReservationsAsync(guestId);

        if (guestReservations.Any(r => r.Status == ReservationStatus.Confirmed && r.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow) && r.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow)))
        {
            throw new Exception("Can't delete account when having active reservation");
        }

        foreach (var reservation in guestReservations)
        {
            notificationSenderService.Send(new NotificationPayload
            {
                EntityId = reservation.Id,
                ReceiverId = reservation.Property.CreatedById,
                SenderId = reservation.CreatedById,
                Type = NotificationType.ReservationResponse
            });
        }

        await reservationRepository.DeleteGuestReservationsAsync(guestId);
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

    public async Task<IEnumerable<Reservation>> GetGuestReservationsAsync(Guid guestId)
    {
        return await reservationRepository.GetGuestReservationsAsync(guestId);
    }

    public async Task<int> GetNumberOfCancelledReservationsAsync(Guid guestId)
    {
        return await reservationRepository.GetNumberOfCancelledReservationsAsync(guestId);
    }
}
