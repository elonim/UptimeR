
namespace UptimeR.ML.Domain.Models.RavenModels;

public class AnomalyLog
{
    public double Alert { get; set; }
    public double Score { get; set; }
    public double PValue { get; set; }
    public DateTime Time { get; set; }
}