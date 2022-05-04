using Microsoft.ML.Data;

namespace UptimeR.ML.Trainer.Models;

public class InputData
{
    [LoadColumn(0)]
    public DateTime Time { get; set; }

    [LoadColumn(1)]
    public float Latency { get; set; }
}