
namespace UptimeR.ML.Domain.Models;

public class Logs
{
    public string ServiceName { get; set; } = "";
    public DateTime Time { get; set; }
    public double Latency { get; set; }
}
