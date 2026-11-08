using CommerceAI.Application.Common.Models;
using CommerceAI.Application.Interfaces;
using CommerceAI.Domain.Entities;

namespace CommerceAI.UnitTests.Fakes;

public class FakeProductRepository
    : IProductRepository
{
    public List<Product> Products { get; } = [];

    public bool SaveChangesCalled { get; private set; }
    public bool ShouldThrowOnSave { get; set; }
    public bool SetOriginalVersionCalled { get; private set; }

    public Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        Products.Add(product);

        return Task.CompletedTask;
    }

    public Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var product = Products.FirstOrDefault(
            x => x.Id == id);

        return Task.FromResult(product);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        if (ShouldThrowOnSave)
        {
            throw new InvalidOperationException(
                "Database failure");
        }

        SaveChangesCalled = true;

        return Task.CompletedTask;
    }

    public void Remove(Product product)
    {
        Products.Remove(product);
    }

    public void SetOriginalVersion(Product product, uint version)
    {
        SetOriginalVersionCalled = true;
    }

    public Task<PaginatedResult<Product>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Products.Count);
    }
}