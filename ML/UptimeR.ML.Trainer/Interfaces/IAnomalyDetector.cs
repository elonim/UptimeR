namespace UptimeR.ML.Trainer.Interfaces;

public interface IAnomalyDetector
{
    void Detect();
    void Detect24Hours(DateTime time);
}
