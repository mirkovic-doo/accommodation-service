namespace AccommodationService.Controllers.Reservation.Requests;

public record ReservationRequest
{
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required int Guests { get; set; }
    public required Guid PropertyId { get; set; }
}
