using AccommodationService.Domain.Base;

namespace AccommodationService.Domain;

public class Property : Entity, IEntity
{
    public Property()
    {
        Name = string.Empty;
    }

    public Property(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
