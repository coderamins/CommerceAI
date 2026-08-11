using CommerceAI.Application.Interfaces;
using CommerceAI.Domain.Entities;

namespace CommerceAI.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product, 
        CancellationToken cancellationToken = default)
    {
        await _context.Products
            .AddAsync(product,cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await _context
            .Products.FindAsync(id, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
