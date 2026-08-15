using CommerceAI.Application.Common.Models;
using CommerceAI.Domain.Entities;

namespace CommerceAI.Application.Interfaces;

public interface IProductRepository
{
    Task AddAsync(
           Product product,
           CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<Product>> GetPagedAsync(
           int pageNumber,
           int pageSize,
           string? search,
           decimal? minPrice,
           decimal? maxPrice,
           string? sortBy,
           string? sortDirection,
           CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        CancellationToken cancellationToken = default);
}
