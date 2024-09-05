namespace AccommodationService.Controllers.AvailabilityPeriod.Requests;

public record AvailabilityPeriodRequest
{
    public Guid? Id { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required decimal PricePerDay { get; set; }
    public required Guid PropertyId { get; set; }
}
