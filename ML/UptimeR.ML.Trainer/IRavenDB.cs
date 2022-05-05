using UptimeR.ML.Trainer.Models;

namespace UptimeR.ML.Trainer;

public interface IRavenDB
{
    void SaveLogs(RavenLog log);
}
