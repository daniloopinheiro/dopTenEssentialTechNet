using Dapper;
using DynamicDapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Defina ConnectionStrings:Default.");

await EnsureDatabaseAndSchemaAsync(connectionString);

var repo = new PostsRepository(connectionString);
var posts = await repo.GetPostsByTitleAsync("Hello Dapper");
foreach (var p in posts)
{
    Console.WriteLine($"#{p.Id} {p.CreationDate:O} — {p.Body}");
}

static async Task EnsureDatabaseAndSchemaAsync(string connectionString)
{
    var builder = new SqlConnectionStringBuilder(connectionString);
    var catalog = builder.InitialCatalog;
    ArgumentException.ThrowIfNullOrEmpty(catalog);
    builder.InitialCatalog = "master";

    await using (var master = new SqlConnection(builder.ConnectionString))
    {
        await master.OpenAsync();
        await master.ExecuteAsync(
            """
            IF DB_ID(@name) IS NULL
            BEGIN
                DECLARE @sql nvarchar(max) = N'CREATE DATABASE ' + QUOTENAME(@name);
                EXEC sp_executesql @sql;
            END
            """,
            new { name = catalog });
    }

    await using var db = new SqlConnection(connectionString);
    await db.OpenAsync();
    await db.ExecuteAsync(
        """
        IF OBJECT_ID(N'Posts', N'U') IS NULL
        BEGIN
            CREATE TABLE Posts (
                Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                Title VARCHAR(200) NOT NULL,
                Body NVARCHAR(MAX) NULL,
                CreationDate DATETIME2 NOT NULL CONSTRAINT DF_Posts_CreationDate DEFAULT (SYSUTCDATETIME())
            );
        END
        """);

    var count = await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Posts WHERE Title = @t", new { t = "Hello Dapper" });
    if (count == 0)
    {
        await db.ExecuteAsync(
            "INSERT INTO Posts (Title, Body) VALUES (@title, @body);",
            new { title = "Hello Dapper", body = "Corpo do post de demonstração." });
    }
}
