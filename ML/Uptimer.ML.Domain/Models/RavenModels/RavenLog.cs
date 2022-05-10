namespace UptimeR.ML.Domain.Models.RavenModels;

public class RavenLog
{
    public string ServiceName { get; set; } = "";
    public DateOnly Date { get; set; } = new();
    public int NumberOfAnomalies { get; set; }
    public List<AnomalyLog> Logs { get; set; } = new();
}
