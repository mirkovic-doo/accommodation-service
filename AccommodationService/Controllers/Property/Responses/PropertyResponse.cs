namespace AccommodationService.Controllers.Property.Responses;

public record PropertyResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public IEnumerable<string> Amenities { get; set; }
    public IEnumerable<string>? Photos { get; set; }
    public int MinGuests { get; set; }
    public int MaxGuests { get; set; }
}
