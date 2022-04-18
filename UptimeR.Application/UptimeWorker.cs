using Mapster;
using System.Diagnostics;
using System.Net.NetworkInformation;
using UptimeR.Application.Commands.LogHistoryReqursts;
using UptimeR.Application.Commands.URLRequests;
using UptimeR.Application.Interfaces;

namespace UptimeR.Application;

public class UptimeWorker : IUptimeWorker   //yeah yeah yeah, I know this class is a bit of a mess, but it runs as a singleton and uses scoped services, so it's fine.
{
    public void Work(IUnitOfWork unitOfWork, IURLUseCases useCases, ILogHistoryUseCases logHistoryUseCase)
    {
        var models = useCases.GetAllUrls().Adapt<List<UpdateURLRequest>>();
        var urls = models.Adapt<List<UpdateURLRequest>>();
        var timer = new Stopwatch();
        try
        {
            foreach (var url in urls)
            {
                timer.Start();

                if (url.LastHitTime.AddMinutes(url.Interval) > DateTime.Now)
                    continue;

                if (url.OnlyPing)
                    url.LastResultOk = PingUrl(url.Url);

                if (!url.OnlyPing)
                    url.LastResultOk = ReadUrl(url.Url);

                timer.Stop();

                UpdateURLToDatabase(useCases, unitOfWork, url);

                var latency = timer.Elapsed.TotalMilliseconds;
                LogToDatabase(logHistoryUseCase, unitOfWork, url, latency);

                timer.Reset();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private void UpdateURLToDatabase(IURLUseCases UseCases, IUnitOfWork unitOfWork, UpdateURLRequest url)
    {
        //update timestamps
        url.LastHitTime = DateTime.Now;
        if (url.LastResultOk)
            url.LastResultTimeOk = DateTime.Now;
        UseCases.UpdateURL(url);
        unitOfWork.SaveChanges();
    }

    private bool ReadUrl(string url)
    {
        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var response = client.GetAsync(url).Result;
            var s = response.StatusCode.ToString();
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

    private void LogToDatabase(ILogHistoryUseCases logHistoryUse, IUnitOfWork unitOfWork, UpdateURLRequest url, double latency)
    {
        var used = TwoBools(url.OnlyPing);


        var log = new CreateLogRequest
        {
            Time = url.LastHitTime,
            URLId = url.Id,
            WasUp = url.LastResultOk,
            Latency = latency,
            UsedPing = used.Ping,
            UsedHttp = used.Http,
            ServiceName = url.ServiceName
        };
        logHistoryUse.AddLog(log);
        unitOfWork.SaveChanges();
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