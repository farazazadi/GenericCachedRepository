using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GenericCachedRepository.Persistence;
using GenericCachedRepository.Products;

var assemblyName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "GenericCachedRepoDemo_";
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(
            builder.Configuration.GetConnectionString("Default"),
            sqliteOptions => sqliteOptions.MigrationsAssembly(assemblyName))
        );

builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.InitializeAsync();
}

app.UseHttpsRedirection();
app.MapProductEndpoints();

app.Run();