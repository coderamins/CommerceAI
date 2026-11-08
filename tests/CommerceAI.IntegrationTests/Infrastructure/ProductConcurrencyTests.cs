using CommerceAI.Domain.Entities;
using CommerceAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace CommerceAI.IntegrationTests.Infrastructure;

public sealed class ProductConcurrencyTests:IntegrationTestBase
{
    public ProductConcurrencyTests(
        CustomWebApplicationFactory factory)
        :base(factory)
    {
        
    }

    [Fact]
    public async Task SaveChanges_WhenProductWasModifiedByAnotherContext_ThrowConcurrencyException()
    {
        //Arrange
        var product = new Product(
            "Concurrency Test Product",
            100m,
            10);

        await Factory.SeedAsync(product);

        await using var scopeA =
            Factory.Services.CreateAsyncScope();

        await using var scopeB =
            Factory.Services.CreateAsyncScope();

        var contextA = scopeA.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var contextB = scopeB.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

        var productA = await contextA.Products
            .SingleAsync(x => x.Id == product.Id);

        var productB = await contextB.Products
            .SingleAsync(x => x.Id == product.Id);

        // Act
        productA.ChangePrice(120m);
        await contextA.SaveChangesAsync();

        // Assert context B still has the old xmin value
        productB.ChangePrice(90m);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => contextB.SaveChangesAsync());

    }
}
