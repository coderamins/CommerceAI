using CommerceAI.Application.Common.Models;
using CommerceAI.Application.Interfaces;
using CommerceAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<PaginatedResult<Product>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? sortBy = null,
        string? sortDirection = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .AsNoTracking()
            .AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Name.Contains(search));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(x =>
                x.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(x =>
                x.Price <= maxPrice.Value);
        }

        // Sorting
        query = sortBy?.ToLowerInvariant() switch
        {
            "price" when sortDirection == "desc"
                => query.OrderByDescending(x => x.Price),

            "price"
                => query.OrderBy(x => x.Price),

            "name" when sortDirection == "desc"
                => query.OrderByDescending(x => x.Name),

            "name"
                => query.OrderBy(x => x.Name),

            _ => query.OrderBy(x => x.Name)
        };

        // Count before pagination
        var totalCount = await query.CountAsync(
            cancellationToken);

        // Pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<Product>(
            items,
            totalCount,
            pageNumber,
            pageSize);
    }


    public async Task<int> CountAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .CountAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
