using System.Diagnostics.Metrics;

namespace CommerceAI.Application.Metrics;

public sealed class CommerceAIMetrics : ICommerceAIMetrics,IDisposable
{
    public const string MeterName = "CommerceAI";
    private readonly Histogram<double> _productOperationDuration;

    private readonly Meter _meter;

    private readonly Counter<long> _prouctCreated;
    private readonly Counter<long> _prouctUpdated;
    private readonly Counter<long> _prouctDeleted;

    public CommerceAIMetrics(IMeterFactory meterFactory)
    {
        _meter = meterFactory.Create(MeterName);

        _prouctCreated = _meter.CreateCounter<long>(
            "commerceai.products.created",
            unit: "{product}",
            description: "Number of successfuly created products");

        _prouctUpdated = _meter.CreateCounter<long>(
            "commerceai.products.updated",
            unit: "{product}",
            description: "Number of successfuly updated products");

        _prouctDeleted = _meter.CreateCounter<long>(
            "commerceai.products.deleted",
            unit: "{product}",
            description: "Number of successfuly deleted products");

        _productOperationDuration = _meter.CreateHistogram<double>(
            "commerceai.products.operation.duration",
            unit: "ms",
            description: "Duration of produt operations");
    }

    public void ProductCreated()
    {
        _prouctCreated.Add(
            1,
            new KeyValuePair<string,object?>("outcome","success"));
    }

    public void ProductUpdated()
    {
        _prouctUpdated.Add(
            1,
            new KeyValuePair<string,object?>("outcome","success"));
    }

    public void ProductDeleted()
    {
        _prouctDeleted.Add(
            1,
             new KeyValuePair<string, object?>("outcome", "success"));
    }

    public void RecordProductOperationDuration(
        string operation,
        double durationInMiliseconds)
    {
        _productOperationDuration.Record(
            durationInMiliseconds,
            new KeyValuePair<string, object?>("operation", operation));
    }

    public void Dispose()
    {
        _meter.Dispose();
    }
}

