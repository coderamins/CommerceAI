using CommerceAI.Application.Common.Models;
using CommerceAI.Application.Queries.Products.GetProductById;
using CommerceAI.Domain.Entities;
using CommerceAI.IntegrationTests.Infrastructure;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CommerceAI.IntegrationTests.Products;

public class GetProductApiTests : IntegrationTestBase
{
    public GetProductApiTests(
        CustomWebApplicationFactory factory) : base(factory)
    {

    }

    [Fact]
    public async Task GetProduct_Should_Return_200_When_Product_Exists()
    {
        //Arrange
        Product product = new Product(
                "Mechanical Keyboard",
                120,
                10
                );

        await Factory.SeedAsync(product);

        //Act
        var response = await Client.GetAsync(
            $"/api/products/{product.Id}");

        //Assert
        Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

    }

    [Fact]
    public async Task GetProducts_Should_Return_Paginated_Result()
    {
        // Arrange        
        await Factory.SeedAsync(
        [
            new Product("Keyboard", 100, 5),
            new Product("Mouse", 50, 10),
            new Product("Monitor", 300, 3)
        ]);

        // Act
        var response = await Client.GetAsync(
            "/api/products?pageNumber=1&pageSize=2");


        // Assert
        response.EnsureSuccessStatusCode();

        var result =
            await response.Content
                .ReadFromJsonAsync<PaginatedResult<ProductResponse>>();

        Assert.NotNull(result);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(3, result.TotalCount);
    }

    [Fact]
    public async Task GetProduct_Sould_Return_404_When_Product_Does_Not_Exist()
    {
        //Arrange
        var id = Guid.NewGuid();

        //Act
        var response = await Client
            .GetAsync($"/api/products/{id}");

        //Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetProducts_Should_Filter_By_Name()
    {
        //Arrnage
        await Factory.SeedAsync(
        [
            new Product("Mechanical Keyboard",120,5),
            new Product("Wireless Mouse",50,10),
            new Product("Gaming Keyboard",150,3)
        ]);

        //Act
        var response = await Client
            .GetAsync($"/api/products?search=Keyboard");

        //Assert
        response.EnsureSuccessStatusCode();

        var result =
            await response.Content
                .ReadFromJsonAsync<PaginatedResult<ProductResponse>>();

        Assert.Equal(2, result!.Items.Count);
    }

    [Fact]
    public async Task GetProducts_Should_Sort_By_Price_Decending()
    {
        //Arrnage
        await Factory.SeedAsync(
        [
            new Product("Keyboard",120,5),
            new Product("Mouse",500,10),
            new Product("Monitor",300,3)
        ]);

        //Act
        var response = await Client
            .GetAsync($"/api/products?sortBy=price&sortDirection=desc");

        //Assert
        response.EnsureSuccessStatusCode();

        var result =
            await response.Content
                .ReadFromJsonAsync<PaginatedResult<ProductResponse>>();

        Assert.Equal("Mouse", result!.Items.First().Name);
    }

}
