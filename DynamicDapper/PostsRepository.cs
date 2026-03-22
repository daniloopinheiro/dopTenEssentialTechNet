using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DynamicDapper;

/// <summary>Consultas com parâmetros dinâmicos (<see cref="DynamicParameters"/>) como no slide.</summary>
public sealed class PostsRepository
{
    private readonly string _connectionString;

    public PostsRepository(string connectionString) =>
        _connectionString = connectionString;

    public async Task<IEnumerable<PostDTO>> GetPostsByTitleAsync(
        string title,
        CancellationToken cancellationToken = default)
    {
        var sql = @"SELECT Id,
                           Body,
                           CreationDate
                    FROM Posts
                    WHERE Title = @title";

        DynamicParameters param = new();
        param.Add("@title", title, DbType.AnsiString);

        await using SqlConnection connection = new(_connectionString);
        return await connection.QueryAsync<PostDTO>(
            new CommandDefinition(sql, param, cancellationToken: cancellationToken));
    }
}
