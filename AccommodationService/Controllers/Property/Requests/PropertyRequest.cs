using AccommodationService.Domain.Enums;

namespace AccommodationService.Controllers.Property.Requests;

public record PropertyRequest
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }
    public required IEnumerable<string> Amenities { get; set; }
    public required IEnumerable<string> Photos { get; set; }
    public required int MinGuests { get; set; }
    public required int MaxGuests { get; set; }
    public required PricingOption PricingOption { get; set; }
}
