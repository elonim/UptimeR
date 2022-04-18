using Microsoft.EntityFrameworkCore;
using UptimeR.Domain;

namespace UptimeR.Application.Interfaces;

public interface IDatabaseContext
{
    DbSet<URL> URLs { get; set; }
    DbSet<LogHistory> LogHistorys { get; set; }
}