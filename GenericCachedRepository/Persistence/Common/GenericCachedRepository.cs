using GenericCachedRepository.DistributedCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace GenericCachedRepository.Persistence.Common;

internal abstract class GenericCachedRepository<T> : IGenericRepository<T> where T : EntityBase
{
    protected readonly AppDbContext DbContext;
    protected readonly IDistributedCache Cache;
    protected readonly string CacheKeyPrefix = $"{typeof(T).FullName}";
    protected readonly string ListCacheKey = $"ListOf({typeof(T).FullName})";


    protected GenericCachedRepository(AppDbContext dbContext, IDistributedCache distributedCache)
    {
        DbContext = dbContext;
        Cache = distributedCache;
    }
    public virtual async Task<T> GetAsync(int id, CancellationToken token = default)
    {
        var key = CacheKeyPrefix + id;
        var cachedEntry = await Cache.GetEntry<T>(key, token);

        if (cachedEntry is not null)
            return cachedEntry;

        var entity = await DbContext.Set<T>().FindAsync(new object[] {id}, token);

        if (entity is not null)
            await Cache.SetEntry(key, entity, token: token);

        return entity;
    }
    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken token = default)
    {
        var cachedEntries = await Cache.GetEntry<IReadOnlyList<T>>(ListCacheKey, token);

        if (cachedEntries is not null)
            return cachedEntries;

        var entities = await DbContext.Set<T>().ToListAsync(token);

        if (entities.Any())
            await Cache.SetEntry(ListCacheKey, entities, token: token);

        return entities;
    }
    public async Task<T> AddAsync(T entity, CancellationToken token = default)
    {
        await DbContext.Set<T>().AddAsync(entity, token);
        
        return entity;
    }
    public async Task UpdateAsync(T entity, CancellationToken token = default)
    {
        DbContext.Set<T>().Update(entity);

        await SyncCacheAsync(entity, token);
    }

    public async Task DeleteAsync(T entity, CancellationToken token = default)
    {
        DbContext.Set<T>().Remove(entity);

        await SyncCacheAsync(entity, token);
    }

    protected virtual async Task SyncCacheAsync(T entity, CancellationToken token = default)
    {
        await Cache.RemoveAsync(CacheKeyPrefix + entity.Id, token);
        await Cache.RemoveAsync(ListCacheKey, token);
    }
}