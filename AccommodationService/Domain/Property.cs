using AccommodationService.Domain.Base;

namespace AccommodationService.Domain;

public class Property : Entity, IEntity
{
    public Property()
    {
        Name = string.Empty;
        Location = string.Empty;
        Amenities = new List<string>();
        Photos = new List<string>();
    }

    public Property(
        string name,
        string location,
        IList<string> amenities,
        IList<string>? photos,
        int minGuests,
        int maxGuests)
    {
        Name = name;
        Location = location;
        Amenities = amenities;
        Photos = photos;
        MinGuests = minGuests;
        MaxGuests = maxGuests;
    }

    public string Name { get; set; }
    public string Location { get; set; }
    public IList<string> Amenities { get; set; }
    public IList<string>? Photos { get; set; }
    public int MinGuests { get; set; }
    public int MaxGuests { get; set; }

}
