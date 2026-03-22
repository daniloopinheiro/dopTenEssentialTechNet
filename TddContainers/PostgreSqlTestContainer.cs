using Testcontainers.PostgreSql;
using Xunit;

namespace TddContainers;

/// <summary>
/// Container PostgreSQL para testes de integração (Testcontainers + xUnit <see cref="IAsyncLifetime"/>).
/// </summary>
public sealed class PostgreSqlTestContainer : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public string ConnectionString => _postgreSqlContainer.GetConnectionString();

    public PostgreSqlTestContainer()
    {
        _postgreSqlContainer = new PostgreSqlBuilder("postgres:15")
            .WithDatabase($"ftc_core_test_{Guid.NewGuid():N}")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithPortBinding(0, true) // Porta aleatória no host para evitar conflitos
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}
