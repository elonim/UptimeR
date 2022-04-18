using UptimeR.Domain;

namespace UptimeR.Application.Interfaces;

public interface ILogHistoryRepository : IRepository<LogHistory>
{
    Task<int> CountLogs();
}