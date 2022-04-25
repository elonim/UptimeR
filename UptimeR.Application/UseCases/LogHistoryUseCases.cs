using Mapster;
using UptimeR.Application.Commands.LogHistoryReqursts;
using UptimeR.Application.Interfaces;
using UptimeR.Domain;

namespace UptimeR.Application.UseCases;

public class LogHistoryUseCases : ILogHistoryUseCases
{
    private readonly ILogHistoryRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    public LogHistoryUseCases(IUnitOfWork unitOfWork, ILogHistoryRepository repo)
    {
        _unitOfWork = unitOfWork;
        _repo = repo;
    }

    async Task<LogItems> ILogHistoryUseCases.CountLogs()
    {
        try
        {
            var count = new LogItems();
            count.LogHistoryCount = await _repo.CountLogs();
            return count;
        }
        catch (Exception)
        {
            throw new Exception("Error getting log history count");
        }
    }

    async Task ILogHistoryUseCases.AddLog(CreateLogRequest command)
    {
        try
        {
            var log = command.Adapt<LogHistory>();
            await _repo.AddAsync(log);
            _unitOfWork.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Error Adding Log");
        }
    }
}