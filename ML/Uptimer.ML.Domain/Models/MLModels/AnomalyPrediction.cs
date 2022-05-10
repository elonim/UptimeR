using Microsoft.ML.Data;

namespace UptimeR.ML.Domain.Models.MLModels;

public class AnomalyPrediction
{
    [VectorType(3)]
    public double[] Prediction { get; set; }
    public DateTime Time { get; set; }
}
