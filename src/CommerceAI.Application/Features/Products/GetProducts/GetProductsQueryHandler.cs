using CommerceAI.Application.Common.Models;
using CommerceAI.Application.Interfaces;
using CommerceAI.Application.Queries.Products.GetProductById;
using MediatR;

namespace CommerceAI.Application.Queries.Products.GetProducts;

public class GetProductsQueryHandler
    : IRequestHandler<
        GetProductsQuery,
        PaginatedResult<ProductResponse>>
{
    private readonly IProductRepository _repository;

    public GetProductsQueryHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResult<ProductResponse>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.MinPrice,
            request.MaxPrice,
            request.SortBy,
            request.SortDirection,
            cancellationToken);

        var items = result.Items
            .Select(product => new ProductResponse(
                product.Id,
                product.Name,
                product.Price,
                product.Stock))
            .ToList();

        return new PaginatedResult<ProductResponse>(
            items,
            result.TotalCount,
            result.PageNumber,
            result.PageSize);
    }
}