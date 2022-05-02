using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UptimeR.Application.Interfaces;
using UptimeR.DTOs;

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
    public async Task<LogCountDTO> Count()
    {
        try
        {
            var logCount = await _logHistoryUseCases.CountLogs();
            return logCount.Adapt<LogCountDTO>();
        }
        catch (Exception)
        {
            throw;
        }
    }
}