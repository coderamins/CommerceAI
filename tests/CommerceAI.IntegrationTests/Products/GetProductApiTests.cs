using CommerceAI.API.Contracts.Products;
using CommerceAI.Application.Common.Models;
using CommerceAI.Application.Queries.Products.GetProductById;
using CommerceAI.Domain.Entities;
using CommerceAI.IntegrationTests.Infrastructure;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            20);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/products/{product.Id}");

        httpRequest.Headers.IfMatch
            .Add(new EntityTagHeaderValue($"\"{product.Version}\""));

        httpRequest.Content = JsonContent.Create(request);

        var response = await Client.SendAsync(httpRequest);

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
            20);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/products/{product.Id}");

        httpRequest.Headers.IfMatch
            .Add(new EntityTagHeaderValue($"\"{product.Version}\""));

        httpRequest.Content = JsonContent.Create(request);

        var updateResponse = await Client.SendAsync(httpRequest);

        updateResponse.StatusCode
            .Should().Be(HttpStatusCode.NoContent);

        var getResponse =
            await Client.GetAsync($"/api/products/{product.Id}");

        getResponse.StatusCode
            .Should().Be(HttpStatusCode.OK);

        var updatedProduct =
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
            20);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/products/{Guid.NewGuid()}");

        httpRequest.Headers.IfMatch.Add(new
            EntityTagHeaderValue($"\"{new uint()}\""));

        httpRequest.Content = JsonContent.Create(request);

        var updateResponse = await Client.SendAsync(httpRequest);

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
            20);

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/products/{product.Id}");

        httpRequest.Content = JsonContent.Create(request);

        httpRequest.Headers.IfMatch.Add(
            new EntityTagHeaderValue(
                $"\"{product.Version}\""));

        // Act
        var response = await Client.SendAsync(httpRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
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
        using var httpRequestA = new HttpRequestMessage(
                HttpMethod.Put,
                $"/api/products/{product.Id}");

        httpRequestA.Content = JsonContent
            .Create(new UpdateProductRequest(
                 "Keyboard Pro",
                110,
                10));

        httpRequestA.Headers.IfMatch.Add(
            new EntityTagHeaderValue($"\"{clientA!.Version}\""));

        var responseA = await Client.SendAsync(httpRequestA);

        Assert.Equal(HttpStatusCode.NoContent, responseA.StatusCode);

        //B tries with stale version
        using var httpRequestB = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/products/{product.Id}");

        httpRequestB.Content = JsonContent
            .Create(new UpdateProductRequest(
                "Keayboard XL",
                120,
                11));

        httpRequestB.Headers.IfMatch.Add(
            new EntityTagHeaderValue($"\"{clientB!.Version}\""));

        var responseB = await Client.SendAsync(httpRequestB);

        Assert.Equal(HttpStatusCode.PreconditionFailed, responseB.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturn_PreconditionFailed_WhenETagsIstale()
    {
        //Arrange
        var product = new Product("Test Product", 100m, 10);
        await Factory.SeedAsync(product);

        //Retrieve the product and its original ETag.
        var getResponse = await Client.GetAsync(
            $"/api/products/{product.Id}");

        getResponse.EnsureSuccessStatusCode();

        var originalETag = getResponse.Headers.ETag;
        Assert.NotNull(originalETag);

        //First update succeed using the original ETag.
        var firstRequest = new UpdateProductRequest(
                    "First Update",
                    120m,
                    10);


        using var firstHttpRequest = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/products/{product.Id}")
        {
            Content = JsonContent.Create(firstRequest)
        };

        firstHttpRequest.Headers.IfMatch.Add(originalETag);

        var firstResponse = await Client.SendAsync(firstHttpRequest);

        Assert.Equal(
            HttpStatusCode.NoContent,
            firstResponse.StatusCode);


        //Second update uses the now-stale ETag.
        var secondRequest = new UpdateProductRequest(
                    "Second Update",
                    90m,
                    10);


        using var secondHttpRequest = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/products/{product.Id}")
        {
            Content = JsonContent.Create(firstRequest)
        };

        secondHttpRequest.Headers.IfMatch.Add(originalETag);

        //Act
        var secondResponse = await Client.SendAsync(secondHttpRequest);

        //Assert
        Assert.Equal(
            HttpStatusCode.PreconditionFailed,
            secondResponse.StatusCode);


        //Verify the first update remains intact.
        var finalResponse = await Client.GetAsync(
            $"/api/products/{product.Id}");

        finalResponse.EnsureSuccessStatusCode();

        var finalProduct = await finalResponse.Content
            .ReadFromJsonAsync<ProductResponse>();

        Assert.NotNull(finalProduct);
        Assert.Equal("First Update", finalProduct.Name);
        Assert.Equal(120m, finalProduct.Price);
    }

    [Fact]
    public async Task GetById_ShouldReturnETag_WhenProductExists()
    {
        var product = new Product("Test Product", 100m, 10);
        await Factory.SeedAsync(product);

        var response = await Client.GetAsync(
            $"/api/products/{product.Id}");

        response.EnsureSuccessStatusCode();

        Assert.NotNull(response.Headers.ETag);
        Assert.False(response.Headers.ETag.IsWeak);
    }

    [Fact]
    public async Task Update_ShouldReturn428_WhenIfMatchIsMissing()
    {
        var product = new Product("Test Product", 100m, 10);
        await Factory.SeedAsync(product);

        var request = new UpdateProductRequest(
            "Updated Product",
            120m,
            10);

        var response = await Client.PutAsJsonAsync(
            $"/api/products/{product.Id}",
            request);

        Assert.Equal(
            HttpStatusCode.PreconditionRequired,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenIfMatchIsMalformed()
    {
        var product = new Product("Test Product", 100m, 10);
        await Factory.SeedAsync(product);

        using var request = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/products/{product.Id}")
        {
            Content = JsonContent.Create(
                new UpdateProductRequest(
                    "Updated Product",
                    120m,
                    10))
        };

        request.Headers.TryAddWithoutValidation(
            "If-Match",
            "not-a-valid-etag");

        var response = await Client.SendAsync(request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

}
