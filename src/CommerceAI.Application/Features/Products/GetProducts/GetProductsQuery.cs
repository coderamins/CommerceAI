using CommerceAI.Application.Common.Models;
using CommerceAI.Application.Queries.Products.GetProductById;
using MediatR;

namespace CommerceAI.Application.Queries.Products.GetProducts;

public record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? SortBy = null,
    string? SortDirection = null
)
: IRequest<PaginatedResult<ProductResponse>>;