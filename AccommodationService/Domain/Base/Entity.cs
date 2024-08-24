namespace AccommodationService.Domain.Base;

public class Entity : IEntity, IAuditedEntity
{
    public Guid Id { get; set; }
    public Guid CreatedById { get; set; }
    public Guid UpdatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
