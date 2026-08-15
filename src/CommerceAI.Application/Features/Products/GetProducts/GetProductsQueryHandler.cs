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
        var products = await _repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var totalCount = await _repository.CountAsync(
            cancellationToken);


        var items = products
            .Select(product =>
                new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Price,
                    product.Stock))
            .ToList();


        return new PaginatedResult<ProductResponse>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}