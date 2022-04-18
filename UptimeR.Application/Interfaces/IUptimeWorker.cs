namespace UptimeR.Application.Interfaces;

public interface IUptimeWorker
{
    void Work(IUnitOfWork unitOfWork, IURLUseCases useCases, ILogHistoryUseCases logHistoryUse);
}
