using Microsoft.EntityFrameworkCore;

namespace ContextPool;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<PoolDemoItem> Items => Set<PoolDemoItem>();
}

public class PoolDemoItem
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
}
