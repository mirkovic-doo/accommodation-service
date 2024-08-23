using AccommodationService.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AccommodationService.Infrastructure.Extensions;

public static class ChangeTrackerExtensions
{
    public static void SetAuditProperties(this ChangeTracker changeTracker, Guid currentUserId)
    {
        changeTracker.DetectChanges();
        var createdEntities = changeTracker
            .Entries()
            .Where(t => t.Entity is IEntity && t.State == EntityState.Added).ToList();

        if (createdEntities.Any())
        {
            foreach (var entry in createdEntities)
            {
                var entity = (IEntity)entry.Entity;
                entity.Id = Guid.NewGuid();
            }
        }

        changeTracker.DetectChanges();
        var createdAuditedEntities = changeTracker
            .Entries()
            .Where(t => t.Entity is IAuditedEntity && t.State == EntityState.Added).ToList();

        if (createdAuditedEntities.Any())
        {
            foreach (var entry in createdAuditedEntities)
            {
                var entity = (IAuditedEntity)entry.Entity;
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedById = currentUserId;
                entity.CreatedById = currentUserId;
            }
        }

        changeTracker.DetectChanges();
        var updatedEntities = changeTracker
            .Entries()
            .Where(t => t.Entity is IAuditedEntity && t.State == EntityState.Modified).ToList();

        if (updatedEntities.Any())
        {
            foreach (var entry in updatedEntities)
            {
                var entity = (IAuditedEntity)entry.Entity;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedById = currentUserId;
            }
        }
    }
}
