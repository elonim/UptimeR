using UptimeR.ML.Trainer.Models;

namespace UptimeR.ML.Trainer.Interfaces;

public interface IRavenDB
{
    void SaveLogs(RavenLog log);
}
