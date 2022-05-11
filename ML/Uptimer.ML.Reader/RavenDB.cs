using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Uptimer.ML.Reader.RavenConnection;
using UptimeR.ML.Domain.Models.RavenModels;
using UptimeR.ML.Trainer.Interfaces;

namespace Uptimer.ML.Reader;

public class RavenDB : IRavenDB
{
    private readonly IConfiguration _config;
    public RavenDB(IConfiguration config)
    {
        _config = config;
    }
    private IDocumentStore CreateStore()
    {
        var connection = GetRavenConnection();
        IDocumentStore store = new DocumentStore()
        {
            Urls = new[] { connection.Url, },
            Conventions =
                {
                    MaxNumberOfRequestsPerSession = 10,
                    UseOptimisticConcurrency = true
                },
            Database = connection.Database
        }.Initialize();
        return store;
    }
    
    private ConnectionRavenDB GetRavenConnection()
    {
        var connection = new ConnectionRavenDB();
        connection.Url = _config.GetSection("RavenConnection").GetSection("Url").Value;
        connection.Database = _config.GetSection("RavenConnection").GetSection("Database").Value;
        return connection;
    }

    public void Save(RavenLog log)
    {
        using var documentStore = CreateStore();
        using var session = documentStore.OpenSession();
        try
        {
            session.Store(log);
            session.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Error 3456");
        }
    }

    public void Save(ServiceAnomalies anomalies)
    {
        using var documentStore = CreateStore();
        using var session = documentStore.OpenSession();
        try
        {
            session.Store(anomalies);
            session.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Error 3456");
        }
    }

    public async Task<ServiceAnomalies> GetAnomaliesForDate(DateOnly date)
    {
        try
        {
            using var documentStore = CreateStore();
            using var session = documentStore.OpenAsyncSession();

            var result = await session
                .Query<ServiceAnomalies>()
                .Search(x => x.Date, new[] { date.ToString("yyyy-MM-dd") })
                .SingleOrDefaultAsync();

            if (result == null)
                return new ServiceAnomalies();
            return result;
        }
        catch (Exception)
        {
            throw new Exception("RavenDB Error");
        }
    }


    public async Task<RavenLog> GetAnomaliesForService(DateOnly date, String serviceName)
    {
        try
        {
            using var documentStore = CreateStore();
            using var session = documentStore.OpenAsyncSession();

            var result = await session
                .Query<RavenLog>()
                .Where(x => x.Date.In(date))
                .Search(x => x.ServiceName, new[] { serviceName })
                .SingleOrDefaultAsync();

            if (result == null)
                return new RavenLog();
            return result;
        }
        catch (Exception)
        {
            throw new Exception("RavenDB Error 333");
        }
    }
}