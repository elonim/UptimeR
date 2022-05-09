using UptimeR.ML.Domain.Models.RavenModels;

namespace UptimeR.ML.Trainer.Interfaces;

public interface IRavenDB
{
    void Save(RavenLog log);
    void Save(ServiceAnomalies anomalies);
}
