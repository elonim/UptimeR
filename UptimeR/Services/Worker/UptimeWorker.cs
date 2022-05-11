using Mapster;
using System.Diagnostics;
using System.Net.NetworkInformation;
using UptimeR.Application.Commands.LogHistoryReqursts;
using UptimeR.Application.Commands.URLRequests;
using UptimeR.Application.Interfaces;

namespace UptimeR.Services.Worker;

public class UptimeWorker : IUptimeWorker
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IURLUseCases _urlUseCases;
    private readonly ILogHistoryUseCases _logHistoryUseCase;
    public UptimeWorker(IUnitOfWork unitOfWork, IURLUseCases urlUseCases, ILogHistoryUseCases logHistoryUseCase)
    {
        _unitOfWork = unitOfWork;
        _urlUseCases = urlUseCases;
        _logHistoryUseCase = logHistoryUseCase;
    }

    public void Work()
    {
        var models = _urlUseCases.GetAllUrls().Adapt<List<UpdateURLRequest>>();
        var urls = models.Adapt<List<UpdateURLRequest>>();
        var timer = new Stopwatch();
        try
        {
            foreach (var url in urls)
            {
                if (url.LastHitTime.AddMinutes(url.Interval) > DateTime.Now)
                    continue;

                timer.Start();

                if (url.OnlyPing)
                    url.LastResultOk = PingUrl(url.Url);

                if (!url.OnlyPing)
                    url.LastResultOk = ReadUrl(url.Url);

                timer.Stop();

                UpdateURLToDatabase(url);

                var latency = timer.Elapsed.TotalMilliseconds;
                LogToDatabase(url, latency);

                timer.Reset();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void UpdateURLToDatabase(UpdateURLRequest url)
    {
        //update timestamps
        url.LastHitTime = DateTime.Now;
        if (url.LastResultOk)
            url.LastResultTimeOk = DateTime.Now;
        _urlUseCases.UpdateURL(url);
        _unitOfWork.SaveChanges();
    }

    private bool ReadUrl(string url)
    {
        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var response = client.GetAsync(url).Result;
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private bool PingUrl(string url)
    {
        try
        {
            var ping = new Ping();
            var reply = ping.Send(url, 5000);
            return reply.Status == IPStatus.Success;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void LogToDatabase(UpdateURLRequest url, double latency)
    {
        var used = TwoBools(url.OnlyPing);

        var log = new CreateLogRequest
        {
            Time = url.LastHitTime,
            URLId = url.Id,
            WasUp = url.LastResultOk,
            ServiceName = url.ServiceName,
            Latency = latency,
            UsedPing = used.Ping,
            UsedHttp = used.Http
        };
        _logHistoryUseCase.AddLog(log);
        _unitOfWork.SaveChanges();
    }

    private (bool Http, bool Ping) TwoBools(bool indput)
    {
        if (indput)
        {
            return (false, true);
        }
        return (true, false);
    }
}