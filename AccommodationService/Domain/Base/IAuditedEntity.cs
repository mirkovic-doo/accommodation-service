namespace AccommodationService.Domain.Base;

public interface IAuditedEntity
{
    Guid CreatedById { get; set; }
    Guid UpdatedById { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
