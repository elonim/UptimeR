using Microsoft.AspNetCore.Mvc;
using UptimeR.Application.Commands.LogHistoryReqursts;
using UptimeR.Application.Interfaces;

namespace UptimeR.Controllers;

public class LogHistoryController
{
    private readonly ILogHistoryUseCases _logHistoryUseCases;
    public LogHistoryController(ILogHistoryUseCases logHistoryUseCases)
    {
        _logHistoryUseCases = logHistoryUseCases;
    }

    [HttpGet]
    [Route("/api/logcount")]
    public async Task<LogItems> Count()
    { 
        return await _logHistoryUseCases.CountLogs();
    }
}