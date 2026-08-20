using Testcontainers.PostgreSql;

namespace CommerceAI.IntegrationTests;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container=
        new PostgreSqlBuilder()
            .WithName("postgres:17-alpine")
            .WithDatabase("commerceaai_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public string ConnectionString =>
        _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }


}
