using CommerceAI.Application.Interfaces;
using CommerceAI.Domain.Entities;
using MediatR;

namespace CommerceAI.Application.Features.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var product = new Product(
                request.Name,
                request.Price,
                request.Stock
            );

        await _productRepository
            .AddAsync(product, cancellationToken);

        await _productRepository.SaveChangesAsync();

        return product.Id;
    }
}
