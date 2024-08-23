using AccommodationService.Domain.Base;
using AccommodationService.Domain.Enums;

namespace AccommodationService.Domain;

public class Property : Entity, IEntity
{
    public Property()
    {
        Name = string.Empty;
        Location = string.Empty;
        Amenities = new List<string>();
        Photos = new List<string>();
        AvailabilityPeriods = new List<AvailabilityPeriod>();
    }

    public Property(
        string name,
        string location,
        IList<string> amenities,
        IList<string>? photos,
        int minGuests,
        int maxGuests,
        PricingOption pricingOption,
        IList<AvailabilityPeriod> availabilityPeriods)
    {
        Name = name;
        Location = location;
        Amenities = amenities;
        Photos = photos;
        MinGuests = minGuests;
        MaxGuests = maxGuests;
        PricingOption = pricingOption;
        AvailabilityPeriods = availabilityPeriods;
    }

    public string Name { get; set; }
    public string Location { get; set; }
    public IList<string> Amenities { get; set; }
    public IList<string>? Photos { get; set; }
    public int MinGuests { get; set; }
    public int MaxGuests { get; set; }
    public PricingOption PricingOption { get; set; }
    public IList<AvailabilityPeriod> AvailabilityPeriods { get; set; }

}
