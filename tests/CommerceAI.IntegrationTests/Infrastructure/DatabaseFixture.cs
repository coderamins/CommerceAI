using CommerceAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommerceAI.IntegrationTests.Infrastructure;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlFixture _postgres;

    public ApplicationDbContext DbContext { get; private set; } = null!;

    public DatabaseFixture(PostgreSqlFixture postgres)
    {
        _postgres = postgres;
    }

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgres.DbContext.Database.GetConnectionString())
            .Options;

        DbContext = new ApplicationDbContext(options);

        await DbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
}