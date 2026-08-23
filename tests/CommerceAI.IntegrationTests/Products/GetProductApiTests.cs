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

public class GetProductApiTests:
    IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public GetProductApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task GetProduct_Should_Return_200_When_Product_Exists()
    {
        //Arrange
        var product = new Product(
                "Mechanical Keyboard",
                120,
                10);

        await _factory.SeedAsync(product);

        //Act
        var response = await _client.GetAsync(
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
        var products = new[]
        {
            new Product("Keyboard", 100, 5),
            new Product("Mouse", 50, 10),
            new Product("Monitor", 300, 3)
        };

        await _factory.SeedAsync(products);


        // Act
        var response = await _client.GetAsync(
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
        var response = await _client
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
        var products = new[]
        {
            new Product("Mechanical Keyboard",120,5),
            new Product("Wireless Mouse",50,10),
            new Product("Gaming Keyboard",150,3)
        };

        await _factory.SeedAsync(products);

        //Act
        var response = await _client
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
        var products = new[]
        {
            new Product("Mechanical Keyboard",100,10),
            new Product("Wireless Mouse",150,30),
            new Product("Gaming Keyboard",130,50),
            new Product("Monitor",135,50),
        };

        await _factory.SeedAsync(products);

        //Act
        var response = await _client
            .GetAsync($"/api/products?sortBy=price&sortDirection=desc");

        //Assert
        response.EnsureSuccessStatusCode();

        var result =
            await response.Content
                .ReadFromJsonAsync<PaginatedResult<ProductResponse>>();

        Assert.Equal("Wireless Mouse", result!.Items.First().Name);
    }

}
