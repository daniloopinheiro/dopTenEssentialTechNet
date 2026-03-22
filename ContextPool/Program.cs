using ContextPool;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Configure ConnectionStrings:DefaultConnection.");

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(defaultConnection));

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    if (!await db.Items.AnyAsync())
    {
        db.Items.Add(new PoolDemoItem { Name = "exemplo-pool" });
        await db.SaveChangesAsync();
    }
}

app.MapGet("/", async (AppDbContext db) =>
{
    var n = await db.Items.CountAsync();
    return Results.Ok(new { message = "DbContext pool", items = n });
});

app.Run();
