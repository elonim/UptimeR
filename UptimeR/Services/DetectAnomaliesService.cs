
using UptimeR.ML.Trainer.Interfaces;

namespace UptimeR.Services;

public class DetectAnomaliesService : IHostedService, IDisposable
{
    private readonly ILogger<DetectAnomaliesService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer _timer = null!;
    private int _timespan = 86400;

    public DetectAnomaliesService(ILogger<DetectAnomaliesService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    private void DoWork(object? state) //yeah yeah yeah, I know this is a bit of a mess, but it runs as a singleton and uses scoped services, so it's fine.
    {
        _logger.LogInformation($"Detecting Anomalies : [{DateTime.Now}]");
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var detector = scope.ServiceProvider.GetRequiredService<IAnomalyDetector>();
            detector.Detect24Hours(date);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"AnomalityDetectorService Startet at : [{DateTime.Now}]");

        //Countdown to midnight
        DateTime timeNow = DateTime.Now;
        DateTime timeMidnight = DateTime.Today.AddDays(1);
        TimeSpan ts = timeMidnight.Subtract(timeNow);
        int secondsToMidnight = (int)ts.TotalSeconds;

        var msUntillRun = secondsToMidnight * 1000 + 300000;

        _logger.LogInformation($"First Anomalitydection will run at : [{DateTime.Now.AddMilliseconds(msUntillRun)}]");
        Thread.Sleep(msUntillRun);

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_timespan));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"AnomalityDetectorService Stopped at : [{DateTime.Now}]");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

}
