using Microsoft.EntityFrameworkCore;

namespace FinnD.Strompreisrechner.Api.Database;

public sealed class HistoryDbContext : DbContext
{
    public DbSet<HistoryEntry> History { get; private set; }

    public HistoryDbContext(DbContextOptions<HistoryDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HistoryEntryConfiguration());
    }
}
