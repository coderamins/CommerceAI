using MediatR;

namespace CommerceAI.Application.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    decimal Price,
    int Stock,
    uint Version) : IRequest;