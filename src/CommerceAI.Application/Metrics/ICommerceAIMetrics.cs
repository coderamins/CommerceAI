namespace CommerceAI.Application.Metrics;

public interface ICommerceAIMetrics
{
    void ProductCreated();

    void ProductUpdated();

    void ProductDeleted();

    void RecordProductOperationDuration(
        string operation,
        double durationInMilliseconds);
}