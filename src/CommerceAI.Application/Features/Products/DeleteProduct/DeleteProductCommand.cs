using MediatR;

namespace CommerceAI.Application.Features.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id):IRequest;
