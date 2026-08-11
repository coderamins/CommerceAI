using MediatR;

namespace CommerceAI.Application.Queries.Products.GetProductById;

public record GetProductByIdQuery(Guid Id)
    : IRequest<ProductResponse?>;