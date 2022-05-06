using Raven.Client.Documents;

namespace Uptimer.ML.Reader;

public interface IReader
{
    Task<ServiceAnomalies> GetAnomaliesForDate(DateOnly date);
}

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
                .Search(x => x.Date, new[]
                 {
                date.ToString("yyyy-MM-dd")
                 })
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
}