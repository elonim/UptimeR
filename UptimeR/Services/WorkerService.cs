using UptimeR.Services.Worker;

namespace UptimeR.Services;
public class WorkerService : IHostedService, IDisposable
{
    private readonly ILogger<WorkerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer _timer = null!;
    private int _timespan = 60;

    public WorkerService(ILogger<WorkerService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    private void DoWork(object? state) //yeah yeah yeah, I know this is a bit of a mess, but it runs as a singleton and uses scoped services, so it's fine.
    {
        _logger.LogInformation($"Backgroundservice Logging Services : [{DateTime.Now}]");
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<IUptimeWorker>();

            worker.Work();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Backgroundservice startet at : [{DateTime.Now}]");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_timespan));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Backgroundservice stopped at : [{DateTime.Now}]");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}