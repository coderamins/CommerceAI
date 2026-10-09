using CommerceAI.Application.Interfaces;
using MediatR;

namespace CommerceAI.Application.Features.Products.DeleteProduct;

public sealed class DeleteProductCommandHandler :
    IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(
        DeleteProductCommand request, 
        CancellationToken cancellationToken)
    {
        var product =await _productRepository
            .GetByIdAsync(request.Id,cancellationToken);

        if(product is null)
        {
            throw new KeyNotFoundException(
                $"Product {request.Id} not found");
        }

        _productRepository.Remove(product);

        await _productRepository.SaveChangesAsync(cancellationToken);
    }
}
