namespace AccommodationService.Controllers.AvailabilityPeriod.Responses;

public record AvailabilityPeriodResponse
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal PricePerDay { get; set; }
    public Guid PropertyId { get; set; }
}
