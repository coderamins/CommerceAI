using MediatR;

namespace CommerceAI.Application.Features.Products.CreateProduct;

public record CreateProductCommand(
        string Name,
        decimal Price,
        int Stock
    ) : IRequest<Guid>;