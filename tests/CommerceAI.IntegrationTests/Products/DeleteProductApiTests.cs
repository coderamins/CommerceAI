using CommerceAI.Domain.Entities;
using CommerceAI.IntegrationTests.Infrastructure;
using FluentAssertions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CommerceAI.IntegrationTests.Products;

public class DeleteProductApiTests:IntegrationTestBase
{
    public DeleteProductApiTests(CustomWebApplicationFactory factory):base(factory)
    {        
    }

    [Fact]
    public async Task Delete_Should_Return_204_When_Product_Exists()
    {
        //Arrange
        var product = new Product(
            "Keyboard",
            120,
            5);

        await Factory.SeedAsync(product);

        //Act
        var response = await Client.DeleteAsync(
            $"/api/products/{product.Id}");

        //Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_Should_Remove_Product_From_Databse()
    {
        //Arrange
        var product = new Product(
            "Keyboard",
            100,
            20);

        await Factory.SeedAsync(product);

        //Delete
        await Client.DeleteAsync(
            $"/api/products/{product.Id}");

        //Try to read it
        var response =await Client.GetAsync(
            $"/api/products/{product.Id}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_Should_Return_404_When_Product_Does_Not_Exist()
    {
        var response =await Client.DeleteAsync(
            $"/api/products/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
