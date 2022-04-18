using Microsoft.EntityFrameworkCore;
using UptimeR.Application.Interfaces;
using UptimeR.Domain;

namespace UptimeR.Persistance;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbContext Context => this;
    public DbSet<URL> URLs { get; set; }
    public DbSet<LogHistory> LogHistorys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}