using UptimeR.Application.Interfaces;

namespace UptimeR.Services.Worker;

public interface IUptimeWorker
{
    void Work(IUnitOfWork unitOfWork, IURLUseCases useCases, ILogHistoryUseCases logHistoryUse);
}