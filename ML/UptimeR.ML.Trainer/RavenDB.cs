using Raven.Client.Documents;
using UptimeR.ML.Trainer.Interfaces;
using UptimeR.ML.Trainer.Models;

namespace UptimeR.ML.Trainer;

public class RavenDB : IRavenDB
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

    public void SaveLogs(RavenLog log)
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
}