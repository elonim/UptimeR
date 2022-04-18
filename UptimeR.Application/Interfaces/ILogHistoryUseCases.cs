
using UptimeR.Application.Commands.LogHistoryReqursts;

namespace UptimeR.Application.Interfaces;

public interface ILogHistoryUseCases
{
    Task AddLog(CreateLogRequest command);
    Task<LogItems> CountLogs();
}
