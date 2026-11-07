using CommerceAI.Application.Interfaces;
using CommerceAI.Application.Metrics;
using CommerceAI.Domain.Entities;
using MediatR;
using System.Diagnostics;

namespace CommerceAI.Application.Features.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly ICommerceAIMetrics _metrics;
    public CreateProductHandler(
        IProductRepository productRepository,
        ICommerceAIMetrics metrics)
    {
        _productRepository = productRepository;
        _metrics = metrics;
    }

    public async Task<Guid> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var sw=Stopwatch.StartNew();

        var product = new Product(
                request.Name,
                request.Price,
                request.Stock
            );

        await _productRepository
            .AddAsync(product, cancellationToken);

        await _productRepository.SaveChangesAsync();

        sw.Stop();
        
        _metrics.ProductCreated();
        _metrics.RecordProductOperationDuration("create", sw.Elapsed.Microseconds);

        return product.Id;
    }
}
