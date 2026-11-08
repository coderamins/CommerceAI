using CommerceAI.Application.Metrics;

namespace CommerceAI.UnitTests.Fakes;

public sealed class FakeCommerceAIMetrics
    : ICommerceAIMetrics
{
    public int ProductsCreated { get; private set; }
    public int ProductsUpdated { get; private set; }
    public int ProductsDeleted { get; private set; }

    public List<string> Operations { get; } = [];

    public void ProductCreated()
    {
        ProductsCreated++;
    }

    public void ProductUpdated()
    {
        ProductsUpdated++;
    }

    public void ProductDeleted()
    {
        ProductsDeleted++;
    }

    public void RecordProductOperationDuration(
        string operation,
        double durationInMilliseconds)
    {
        Operations.Add(operation);
    }
}