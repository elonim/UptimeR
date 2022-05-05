using Raven.Client.Documents;

namespace Uptimer.ML.Reader;

public interface IReader
{
    ServiceAnomalies GetAllCustomers();
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


    public ServiceAnomalies GetAllCustomers()
    {
        using var documentStore = CreateStore();
        using var session = documentStore.OpenSession();
        return session.Load<ServiceAnomalies>("ServiceAnomalies/3-A");
    }
}