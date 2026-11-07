using CommerceAI.Application.Interfaces;
using CommerceAI.Application.Metrics;
using MediatR;
using System.Diagnostics;

namespace CommerceAI.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICommerceAIMetrics _metrics;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        ICommerceAIMetrics metrics)
    {
        _productRepository = productRepository;
        _metrics = metrics;
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

        var sw = Stopwatch.StartNew();

        _productRepository.SetOriginalVersion(
                product,
                request.Version);

        product.Update(
            request.Name,
            request.Price,
            request.Stock);

        await _productRepository.SaveChangesAsync(
            cancellationToken);

        sw.Stop();

        _metrics.RecordProductOperationDuration("update", sw.Elapsed.Microseconds);
        _metrics.ProductUpdated();
    }
}
