using Microsoft.ML.Data;

namespace UptimeR.ML.Trainer.Models;

public class AnomalyPrediction
{
    [VectorType(3)]
    public double[]? Prediction { get; set; }
    public DateTime Time { get; set; }
}
