namespace PrepareData.Models;

public class AvgLogs
{
    public string ServiceName { get; set; } = "";
    public DateTime Time { get; set; }
    public bool UsedPing { get; set; }
    public bool UsedHttp { get; set; }
    public double Latency { get; set; }
    public int UP100 { get; set; }
    public double AvgUpTime { get; set; }
}