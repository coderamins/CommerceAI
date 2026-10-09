using CommerceAI.Application.Interfaces;
using MediatR;

namespace CommerceAI.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(
        UpdateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetByIdAsync(request.Id, cancellationToken);

        if(product is null)
        {
            throw new KeyNotFoundException(
                $"Product with id {request.Id} was not found");
        }

        _productRepository.SetOriginalVersion(
                product,
                request.Version);

        product.Update(
            request.Name,
            request.Price,
            request.Stock);

        await _productRepository.SaveChangesAsync(
            cancellationToken);
    }
}
