using GenericCachedRepository.Persistence;

namespace GenericCachedRepository.Products;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products/{id}", GetProductAsync);
        app.MapGet("/products", GetAllProductsAsync);
        app.MapPost("/products", CreateProductAsync);
        app.MapPut("/products/{id}", UpdateProductAsync);
        app.MapDelete("/products/{id}", DeleteProductAsync);
    }


    private static async Task<IResult> GetProductAsync(int id, ProductRepository repository, CancellationToken token)
    {
        var product = await repository.GetAsync(id, token);

        return product is not null ? Results.Ok(product.ToDto()) : Results.NotFound();
    }

    private static async Task<IReadOnlyList<ProductDto>> GetAllProductsAsync(ProductRepository repository, CancellationToken token)
    {
        var entities = await repository.GetAllAsync(token);
        return entities.ToDto();
    }

    private static async Task<IResult> CreateProductAsync(ProductDto productDto,
        AppDbContext dbContext, ProductRepository repository, CancellationToken token)
    {
        // Validation...

        var product = productDto.ToEntity();
        
        await repository.AddAsync(product, token);
        await dbContext.SaveChangesAsync(token);

        return Results.Created($"/products/{product.Id}", product);
    }

    private static async Task<IResult> UpdateProductAsync(int id, ProductDto productDto,
        AppDbContext dbContext, ProductRepository repository, CancellationToken token)
    {
        // Validation...

        var product = await repository.GetAsync(id, token);

        if (product is null)
            return Results.NotFound();
        

        product.GetFromDto(productDto);

        await repository.UpdateAsync(product, token);

        await dbContext.SaveChangesAsync(token);

        return Results.NoContent();
    }

    private static async Task<IResult> DeleteProductAsync(int id,
        AppDbContext dbContext, ProductRepository repository, CancellationToken token)
    {
        var product = await repository.GetAsync(id, token);

        if (product is null)
            return Results.NotFound();

        await repository.DeleteAsync(product, token);

        await dbContext.SaveChangesAsync(token);

        return Results.NoContent();
    }
}