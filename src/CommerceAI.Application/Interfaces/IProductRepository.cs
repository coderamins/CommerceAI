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
}
