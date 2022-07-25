using GenericCachedRepository.Products;
using Microsoft.EntityFrameworkCore;

namespace GenericCachedRepository.Persistence;

internal class AppDbContext : DbContext
{
    internal DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(product =>
        {
            product.HasKey(p => p.Id);

            product.Property(p=> p.Name)
            .HasMaxLength(255)
            .IsRequired();

            product.Property(p => p.ProducerName)
            .HasMaxLength(100)
            .IsRequired();

            product.Property(p => p.Price)
            .IsRequired();
        });

        base.OnModelCreating(modelBuilder);

    }

}
