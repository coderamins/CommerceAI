using CommerceAI.Application.Features.Products.CreateProduct;
using CommerceAI.UnitTests.Fakes;
using FluentAssertions;

namespace CommerceAI.UnitTests.Products;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_RecordMetrics_When_ProductIsCreated()
    {
        // Arrange
        var repository = new FakeProductRepository();
        var metrics = new FakeCommerceAIMetrics();

        var handler = new CreateProductHandler(
            repository,
            metrics);

        var command = new CreateProductCommand(
            "Mechanical Keyboard",
            120,
            10);

        // Act
        await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        repository.SaveChangesCalled.Should().BeTrue();

        metrics.ProductsCreated.Should().Be(1);

        metrics.Operations.Should().Contain("create");
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

        var handler = new CreateProductHandler(
            repository,
            metrics);

        var command = new CreateProductCommand(
            "Mechanical Keyboard",
            120,
            10);

        // Act
        var action = () => handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>();

        metrics.ProductsCreated.Should().Be(0);

        metrics.Operations.Should().BeEmpty();
    }
}