using AccommodationService.Domain.Base;

namespace AccommodationService.Application.Repositories;

public interface IBaseRepository<T> where T : class, IEntity, IAuditedEntity
{
    Task<T> GetAsync(Guid id);
    Task<T> AddAsync(T entity);
    T Update(T entity);
    void Delete(T entity);
}
