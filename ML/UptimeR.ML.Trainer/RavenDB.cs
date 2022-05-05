using Raven.Client.Documents;
using UptimeR.ML.Trainer.Models;

namespace UptimeR.ML.Trainer;

public class RavenDB : IRavenDB
{
    private IDocumentStore CreateStore()
    {
        IDocumentStore store = new DocumentStore()
        {
            // Define the cluster node URLs (required)
            Urls = new[] { "http://localhost:8080", 
                    /*some additional nodes of this cluster*/ },

            // Set conventions as necessary (optional)
            Conventions =
                {
                    MaxNumberOfRequestsPerSession = 10,
                    UseOptimisticConcurrency = false
                },
            // Define a default database (optional)
            Database = "UptimeR",

            // Define a client certificate (optional)
            //Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx"),

            // Initialize the Document Store
        }.Initialize();
        // ***  Opret indeks
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
        catch (Exception e)
        {
            throw new Exception("Error 3456");
        }
    }
}
