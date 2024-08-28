using AccommodationService.Domain.Enums;

namespace AccommodationService.Controllers.Reservation.Responses;

public record ReservationResponse
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Guests { get; set; }
    public ReservationStatus Status { get; set; }
    public decimal Price { get; set; }
    public Guid PropertyId { get; set; }
    public Guid CreatedById { get; set; }
    public Guid UpdatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
