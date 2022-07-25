using GenericCachedRepository.Persistence.Common;

namespace GenericCachedRepository.Products;

internal class Product : EntityBase
{
    public string Name { get; set; }
    public string ProducerName { get; set; }
    public double Price { get; set; }
}