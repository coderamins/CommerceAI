using CommerceAI.Domain.Entities;
using CommerceAI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;


namespace CommerceAI.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private DatabaseResetter? _databaseResetter;

    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("commerceai_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        // Force WebApplicationFactory to start the application
        _ = Services;

        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();

        _databaseResetter =
            await DatabaseResetter.CreateAsync(
                _postgres.GetConnectionString());
    }

    public async Task ResetDatabaseAsync()
    {
        if (_databaseResetter is null)
        {
            throw new InvalidOperationException(
                "Database resetter has not been initialized.");
        }

        await _databaseResetter.ResetAsync();
    }

    public async Task SeedAsync(Product product)
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await dbContext.Products.AddAsync(product);

        await dbContext.SaveChangesAsync();
    }

    public async Task SeedAsync(
        IEnumerable<Product> products)
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await dbContext.Products.AddRangeAsync(products);

        await dbContext.SaveChangesAsync();
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                service =>
                    service.ServiceType ==
                    typeof(
                        DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseNpgsql(
                        _postgres.GetConnectionString());
                });
        });
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }
}

