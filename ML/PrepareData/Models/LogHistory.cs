namespace PrepareData.Models;

public class LogHistory
{

    public Guid Id { get; set; }
    public Guid URLId { get; set; }
    public string ServiceName { get; set; } = "";
    public DateTime Time { get; set; }
    public bool WasUp { get; set; }
    public bool UsedPing { get; set; }
    public bool UsedHttp { get; set; }
    public double Latency { get; set; }
}