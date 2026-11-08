using CommerceAI.IntegrationTests.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CommerceAI.IntegrationTests;

public sealed class HealthCheckTests:IntegrationTestBase
{
    public HealthCheckTests(CustomWebApplicationFactory factory)
        :base(factory)
    {
        
    }

    [Fact]
    public async Task Live_WhenRequested_ReturnsHealthy()
    {
        // Act
        var response = await Client.GetAsync("/health/live");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", content);
    }

    [Fact]
    public async Task Ready_WhenDatabaseIsAvailable_ReturnsHealthy()
    {
        // Act
        var response = await Client.GetAsync("/health/ready");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", content);
    }
}
