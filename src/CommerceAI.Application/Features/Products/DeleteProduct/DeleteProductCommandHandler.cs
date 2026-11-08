using CommerceAI.Application.Interfaces;
using CommerceAI.Application.Metrics;
using MediatR;
using System.Diagnostics;

namespace CommerceAI.Application.Features.Products.DeleteProduct;

public sealed class DeleteProductCommandHandler :
    IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICommerceAIMetrics _metrics;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        ICommerceAIMetrics metrics)
    {
        _productRepository = productRepository;
        _metrics = metrics;
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

        var sw = Stopwatch.StartNew();

        _productRepository.Remove(product);

        await _productRepository.SaveChangesAsync(cancellationToken);

        sw.Stop();

        _metrics.RecordProductOperationDuration("delete", sw.Elapsed.Microseconds);
        _metrics.ProductDeleted();
    }
}
