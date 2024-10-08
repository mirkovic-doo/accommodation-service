﻿using AccommodationService.Domain.Enums;

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
    public PricingOption PricingOption { get; set; }
    public bool AutoConfirmReservation { get; set; }
    public Guid CreatedById { get; set; }
    public Guid UpdatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
