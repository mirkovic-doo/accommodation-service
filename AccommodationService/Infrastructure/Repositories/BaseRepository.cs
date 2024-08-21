using AccommodationService.Application.Repositories;
using AccommodationService.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity, IAuditedEntity
{
    protected readonly AccommodationDbContext dbContext;

    public BaseRepository(AccommodationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<T> GetAsync(Guid id)
    {
        var entity = await dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            throw new Exception($"Entity with {id} not found");
        }

        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        var entityEntry = await dbContext.Set<T>().AddAsync(entity);
        await SaveChangesAsync();
        return entityEntry.Entity;
    }

    public T Update(T entity)
    {
        var entityEntry = dbContext.Set<T>().Update(entity);
        SaveChanges();
        return entityEntry.Entity;
    }

    public void Delete(T entity)
    {
        dbContext.Remove(entity);
        SaveChanges();
    }

    protected async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    protected void SaveChanges()
    {
        dbContext.SaveChanges();
    }
}
