using Npgsql;
using Xunit;

namespace TddContainers;

/// <summary>Exemplo de teste usando o fixture com container PostgreSQL (requer Docker em execução).</summary>
public sealed class PostgreSqlIntegrationTests : IClassFixture<PostgreSqlTestContainer>
{
    private readonly PostgreSqlTestContainer _container;

    public PostgreSqlIntegrationTests(PostgreSqlTestContainer container)
    {
        _container = container;
    }

    [Fact]
    public async Task ConnectionString_abre_conexao_com_o_banco()
    {
        await using var conn = new NpgsqlConnection(_container.ConnectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT 1", conn);
        var scalar = await cmd.ExecuteScalarAsync();
        Assert.Equal(1, Convert.ToInt32(scalar));
    }
}
