using GenericCachedRepository.Persistence;
using GenericCachedRepository.Persistence.Common;
using Microsoft.Extensions.Caching.Distributed;

namespace GenericCachedRepository.Products;

internal class ProductRepository : GenericCachedRepository<Product>
{
    public ProductRepository(AppDbContext dbContext, IDistributedCache distributedCache) : base(dbContext, distributedCache)
    {
    }
}
