using CommerceAI.Application.Features.Products.UpdateProduct;
using CommerceAI.Domain.Entities;
using CommerceAI.UnitTests.Fakes;
using FluentAssertions;

namespace CommerceAI.UnitTests.Products;

public class UpdateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_RecordMetrics_When_ProductIsUpdated()
    {
        // Arrange
        var repository = new FakeProductRepository();
        var metrics = new FakeCommerceAIMetrics();

        var product = new Product(
            "Old Keyboard",
            100,
            5);

        repository.Products.Add(product);

        var handler = new UpdateProductCommandHandler(
            repository,
            metrics);

        var command = new UpdateProductCommand(
            product.Id,
            "New Keyboard",
            150,
            10,
            1); // Replace with the actual Version type/value if needed.

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repository.SaveChangesCalled.Should().BeTrue();
        repository.SetOriginalVersionCalled.Should().BeTrue();

        metrics.ProductsUpdated.Should().Be(1);
        metrics.Operations.Should().Contain("update");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_ProductDoesNotExist()
    {
        // Arrange
        var repository = new FakeProductRepository();
        var metrics = new FakeCommerceAIMetrics();

        var handler = new UpdateProductCommandHandler(
            repository,
            metrics);

        var command = new UpdateProductCommand(
            Guid.NewGuid(),
            "New Keyboard",
            150,
            10,
            1);

        // Act
        var action = () => handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        await action.Should()
            .ThrowAsync<KeyNotFoundException>();

        metrics.ProductsUpdated.Should().Be(0);
        metrics.Operations.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Not_RecordMetrics_When_SaveFails()
    {
        // Arrange
        var repository = new FakeProductRepository
        {
            ShouldThrowOnSave = true
        };

        var metrics = new FakeCommerceAIMetrics();

        var product = new Product(
            "Old Keyboard",
            100,
            5);

        repository.Products.Add(product);

        var handler = new UpdateProductCommandHandler(
            repository,
            metrics);

        var command = new UpdateProductCommand(
            product.Id,
            "New Keyboard",
            150,
            10,
            1);

        // Act
        var action = () => handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>();

        metrics.ProductsUpdated.Should().Be(0);
        metrics.Operations.Should().BeEmpty();
    }
}