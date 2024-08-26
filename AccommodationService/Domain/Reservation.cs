using AccommodationService.Domain.Base;
using AccommodationService.Domain.Enums;

namespace AccommodationService.Domain;

public class Reservation : Entity, IEntity
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Guests { get; set; }
    public ReservationStatus Status { get; set; }
    public decimal Price { get; set; }

    public Guid PropertyId { get; set; }
    public Property Property { get; set; }
}
