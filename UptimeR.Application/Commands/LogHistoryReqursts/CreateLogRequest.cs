
namespace UptimeR.Application.Commands.LogHistoryReqursts;

public class CreateLogRequest
{
    public string ServiceName { get; set; } = "Unknown";
    public Guid URLId { get; set; }
    public DateTime Time { get; set; }
    public double Latency { get; set; }
    public bool WasUp { get; set; }
    public bool UsedPing { get; set; }
    public bool UsedHttp { get; set; }
}
