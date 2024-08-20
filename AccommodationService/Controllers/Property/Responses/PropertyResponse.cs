namespace AccommodationService.Controllers.Property.Responses;

public record PropertyResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
