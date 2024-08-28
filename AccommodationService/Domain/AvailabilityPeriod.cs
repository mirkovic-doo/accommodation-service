using AccommodationService.Domain.Base;

namespace AccommodationService.Domain;

public class AvailabilityPeriod : Entity, IEntity
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal PricePerDay { get; set; }
    public Guid PropertyId { get; set; }
    public Property Property { get; set; }
}
