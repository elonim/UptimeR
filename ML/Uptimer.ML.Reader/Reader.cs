using Raven.Client.Documents;
using UptimeR.ML.Domain.Models.RavenModels;

namespace Uptimer.ML.Reader;

public class Reader : IReader
{
    private IDocumentStore CreateStore()
    {
        IDocumentStore store = new DocumentStore()
        {
            Urls = new[] { "http://localhost:8080", },
            Conventions =
                {
                    MaxNumberOfRequestsPerSession = 10,
                    UseOptimisticConcurrency = true
                },
            Database = "UptimeR"
        }.Initialize();
        return store;
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
                .Search(x => x.Date, new[] { date.ToString("yyyy-MM-dd") })
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