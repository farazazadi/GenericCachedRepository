namespace GenericCachedRepository.Products;

internal static class Mappers
{
    internal static void GetFromDto(this Product product, ProductDto productDto)
    {
        product.Name = productDto.Name;
        product.ProducerName = productDto.ProducerName;
        product.Price = productDto.Price;
    }
    
    internal static ProductDto ToDto(this Product entity) =>  new
    (
        entity.Id,
        entity.Name,
        entity.ProducerName,
        entity.Price
    );

    internal static IReadOnlyList<ProductDto> ToDto(this IEnumerable<Product> entities)=> 
        entities.Select(product => product.ToDto()).ToList();


    internal static Product ToEntity(this ProductDto productDto) => new()
    {
        Name = productDto.Name,
        ProducerName = productDto.ProducerName,
        Price = productDto.Price
    };
}