using CommerceAI.Domain.Entities;
using CommerceAI.Infrastructure.Persistence.Repositories;
using CommerceAI.IntegrationTests.Infrastructure;

namespace CommerceAI.IntegrationTests.Products;

public class ProductRepositoryTest
    : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;
    public ProductRepositoryTest(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Product()
    {
        var product = new Product(
                "Mechanical Keyboard",
                120,
                100
            );

        _fixture.DbContext.Products.Add(product);
        await _fixture.DbContext.SaveChangesAsync();

        var repository = new ProductRepository(
            _fixture.DbContext);

        //Act
        var result = await repository
            .GetByIdAsync(product.Id);


        //Assert
        Assert.NotNull(result);
        Assert.Equal(
                "Mechanical Keyboard",
                result.Name
            );
    }
}
