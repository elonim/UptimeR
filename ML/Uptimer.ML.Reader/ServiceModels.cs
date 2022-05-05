
namespace Uptimer.ML.Reader;

    public class ServiceAnomalies
    {
        public DateOnly Date { get; set; }
        public List<Anomaly> Anomalies { get; set; } = new();
    }

    public class Anomaly
    {
        public string Servicename { get; set; }
        public int AnomalyCount { get; set; }
    }