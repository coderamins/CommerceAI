using CommerceAI.IntegrationTests.Infrastructure;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CommerceAI.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase
    : IClassFixture<CustomWebApplicationFactory>,
      IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    protected IntegrationTestBase(
        CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
