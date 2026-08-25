using Npgsql;
using Respawn;
using System.Threading.Tasks;

namespace CommerceAI.IntegrationTests.Infrastructure;

public sealed class DatabaseResetter
{
    private readonly Respawner _respawner;
    private readonly string _connectionString;

    private DatabaseResetter(
        Respawner respawner,
        string connectionString)
    {
        _respawner = respawner;
        _connectionString = connectionString;
    }

    public static async Task<DatabaseResetter> CreateAsync(
        string connectionString)
    {
        await using var connection =
            new NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        var respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres
            });

        return new DatabaseResetter(
            respawner,
            connectionString);
    }

    public async Task ResetAsync()
    {
        await using var connection =
            new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        await _respawner.ResetAsync(connection);
    }
}
