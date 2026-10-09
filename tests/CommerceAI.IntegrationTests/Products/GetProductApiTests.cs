using CommerceAI.API.Contracts.Products;
using CommerceAI.Application.Common.Models;
using CommerceAI.Application.Queries.Products.GetProductById;
using CommerceAI.Domain.Entities;
using CommerceAI.IntegrationTests.Infrastructure;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
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

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenProductExists()
    {
        var product = new Product(
            "Old Product",
            100,
            10);    

        await Factory.SeedAsync(product);

        var request = new UpdateProductRequest(
            "Updated Product",
            150,
            20,
            product.Version);

        var response = await Client.PutAsJsonAsync(
            $"/api/products/{product.Id}",
            request);

        response.StatusCode
            .Should()       
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Update_ShouldPersistChanges()
    {
        var product = new Product(
            "Old Product",
            100,
            10);

        await Factory.SeedAsync(product);

        var request = new UpdateProductRequest(
            "Updated Product",
            200,
            20,
            product.Version);

        var updateResponse = await Client.PutAsJsonAsync(
            $"/api/products/{product.Id}",
            request);

        updateResponse.StatusCode
            .Should().Be(HttpStatusCode.NoContent);

        var getResponse =
            await Client.GetAsync($"/api/products/{product.Id}");

        getResponse.StatusCode
            .Should().Be(HttpStatusCode.OK);

        var updatedProduct=
            await getResponse.Content
            .ReadFromJsonAsync<ProductResponse>();

        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be("Updated Product");
        updatedProduct.Price.Should().Be(200);
        updatedProduct.Stock.Should().Be(20);
    }

    [Fact]
    public async Task Update_ShrouldReturn_NotFound_WhenProductDoesNotExist()
    {
        var request = new UpdateProductRequest(
            "Product 1",
            100,
            20,
            new uint());

        var updateResponse = await Client.PutAsJsonAsync(
            $"/api/products/{Guid.NewGuid()}",request);

        updateResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenPriceIsInvalid()
    {
        var product = new Product(
            "product",
            100,
            10);

        await Factory.SeedAsync(product);

        var request = new UpdateProductRequest(
            "Updated Product",
            0,
            20,
            product.Version);

        var updateResponse =
            await Client
            .PutAsJsonAsync($"/api/products/{product.Id}", request);

        updateResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);   
    }


    [Fact]
    public async Task Update_Should_Return_409_When_Product_Was_Modified()
    {
        //Arrange
        var product = new Product("Keyboard", 100, 10);

        await Factory.SeedAsync(product);

        //Client A reads
        var clientA = await Client.GetFromJsonAsync<ProductResponse>(
            $"/api/products/{product.Id}");

        //Client B reads
        var clientB = await Client.GetFromJsonAsync<ProductResponse>(
            $"/api/products/{product.Id}");

        //A updates successfully
        var responseA = await Client.PutAsJsonAsync(
            $"/api/products/{product.Id}",
            new UpdateProductRequest
            (
                "Keyboard Pro",
                110,
                10,
                clientA!.Version
            ));

        Assert.Equal(HttpStatusCode.NoContent, responseA.StatusCode);

        //B tries with stale version
        var responseB = await Client.PutAsJsonAsync(
            $"/api/products/{product.Id}",
            new UpdateProductRequest(
                "Keayboard XL",
                120,
                11,
                clientB!.Version));

        Assert.Equal(HttpStatusCode.Conflict,responseB.StatusCode);

    }
}
