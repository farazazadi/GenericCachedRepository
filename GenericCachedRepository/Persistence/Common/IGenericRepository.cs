namespace GenericCachedRepository.Persistence.Common;

internal interface IGenericRepository<T> where T : EntityBase
{
    Task<T> GetAsync(int id, CancellationToken token);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken token);
    Task<T> AddAsync(T entity, CancellationToken token);
    Task UpdateAsync(T entity, CancellationToken token);
    Task DeleteAsync(T entity, CancellationToken token);
}
