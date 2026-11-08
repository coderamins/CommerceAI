using CommerceAI.UnitTests.Fakes;

public sealed class FailingProductRepository
    : FakeProductRepository
{
    public new Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException(
            "Database failure");
    }
}