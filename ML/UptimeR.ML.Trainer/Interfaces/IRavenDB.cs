using UptimeR.ML.Trainer.Models;

namespace UptimeR.ML.Trainer.Interfaces;

public interface IRavenDB
{
    void Save(RavenLog log);
    void Save(ServiceAnomalies anomalies);
}
