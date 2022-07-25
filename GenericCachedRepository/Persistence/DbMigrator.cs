using GenericCachedRepository.Products;
using Microsoft.EntityFrameworkCore;

namespace GenericCachedRepository.Persistence;

internal static class DbInitializer
{
    internal static async Task InitializeAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await MigrateAsync(dbContext);
        await SeedAsync(dbContext);
    }
    
    private static async Task MigrateAsync(AppDbContext dbContext) => await dbContext.Database.MigrateAsync();

    private static async Task SeedAsync(AppDbContext dbContext)
    {
        if(dbContext.Products.Any())
            return;

        var products = new List<Product>()
        {
            new()
            {
                Name = "JetBrains Rider",
                ProducerName = "JetBrains",
                Price = 50.00
            },
            new()
            {
                Name = "JetBrains PyCharm",
                ProducerName = "JetBrains",
                Price = 20.00
            },
            
            new()
            {
                Name = "Visual Studio Code",
                ProducerName = "Microsoft",
                Price = 0
            },
        };
        
        await dbContext.AddRangeAsync(products);

        await dbContext.SaveChangesAsync();
    }
}
