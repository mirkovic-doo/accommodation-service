namespace AccommodationService.Controllers.Property.Responses;

public record SearchPropertyResponse : PropertyResponse
{
    public decimal TotalPrice { get; set; }
    public decimal UnitPrice { get; set; }
}
