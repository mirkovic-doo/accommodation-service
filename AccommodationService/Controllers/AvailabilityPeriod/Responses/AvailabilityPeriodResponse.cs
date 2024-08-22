namespace AccommodationService.Controllers.AvailabilityPeriod.Responses;

public record AvailabilityPeriodResponse
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal PricePerDay { get; set; }
    public Guid PropertyId { get; set; }
}
