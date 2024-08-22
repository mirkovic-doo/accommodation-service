namespace AccommodationService.Controllers.AvailabilityPeriod.Requests;

public record AvailabilityPeriodRequest
{
    public Guid? Id { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required decimal PricePerDay { get; set; }
    public required Guid PropertyId { get; set; }
}
