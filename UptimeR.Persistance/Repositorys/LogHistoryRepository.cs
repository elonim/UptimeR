using Microsoft.EntityFrameworkCore;
using UptimeR.Application.Interfaces;
using UptimeR.Domain;


namespace UptimeR.Persistance.Repositorys;

public class LogHistoryRepository : Repository<LogHistory>, ILogHistoryRepository
{
    private readonly IDatabaseContext _context;
    public LogHistoryRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }
    public async Task<int> CountLogs()
    {
        return await _context.LogHistorys.CountAsync();
    }
}