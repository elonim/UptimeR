using System.Data.SqlClient;
using Dapper;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using UptimeR.ML.Trainer.Models;

namespace UptimeR.ML.Trainer;

public class AnomalyDetector : IAnomalyDetector
{
    private readonly IConfiguration _config;
    private readonly IRavenDB _ravenDB;
    public AnomalyDetector(IConfiguration config, IRavenDB ravenDB)
    {
        _ravenDB = ravenDB;
        _config = config;
    }

    public void Detect()
    {
        var connString = _config.GetConnectionString("DataConnection");
        var conn = new SqlConnection(connString);
        if (TestConn(conn))
            Work(conn);
    }

    private void Work(SqlConnection conn)
    {
        var allLogs = conn.Query<Logs>("select Time,Latency,ServiceName from LogHistorys");
        var allSortedLogs = SplitLogs(allLogs);
        Calculate(allSortedLogs);
    }

    private void Calculate(SortedLogs logs)
    {
        Parallel.ForEach(logs.Logs, list =>
        {
            var logdata = list.Adapt<List<InputData>>();
            var mlContext = new MLContext();
            IDataView dataView = mlContext.Data.LoadFromEnumerable<InputData>(logdata);
            var predictions = DetectSpike(mlContext, list.Count(), dataView);

            var ravenlog = new RavenLog();
            try
            {
                var anomalyLogs = new List<AnomalyLog>();
                foreach (var p in predictions)
                {
                    ravenlog.Logs.Add(new AnomalyLog
                    {
                        Alert = p.Prediction[0],
                        Score = p.Prediction[1],
                        PValue = p.Prediction[2],
                        Time = p.Time
                    }
                    );
                }
                ravenlog.ServiceName = list[0].ServiceName;
            }
            catch (Exception)
            {
                throw new Exception("Convert Error");
            }
            _ravenDB.SaveLogs(ravenlog);
        });
    }



    private SortedLogs SplitLogs(IEnumerable<Logs> logs)
    {
        try
        {
            var splittedLogs = new SortedLogs();

            foreach (var log in logs.GroupBy(x => x.ServiceName))
            {
                splittedLogs.Logs.Add(log.ToList());
            }
            return splittedLogs;
        }
        catch (Exception)
        {
            throw new Exception("Error 665");
        }
    }

    private IEnumerable<AnomalyPrediction> DetectSpike(MLContext mlContext, int docSize, IDataView productSales)
    {
        try
        {
            var iidSpikeEstimator = mlContext.Transforms.DetectIidSpike(outputColumnName: nameof(AnomalyPrediction.Prediction), inputColumnName: nameof(InputData.Latency), confidence: 99.0, pvalueHistoryLength: docSize / 4);

            ITransformer iidSpikeTransform = iidSpikeEstimator.Fit(CreateEmptyDataView(mlContext));
            IDataView transformedData = iidSpikeTransform.Transform(productSales);
            var predictions = mlContext.Data.CreateEnumerable<AnomalyPrediction>(transformedData, reuseRowObject: false);
            return predictions;
        }
        catch (Exception)
        {
            throw new Exception("Error 777");
        }
    }

    private IDataView CreateEmptyDataView(MLContext mlContext)
    {
        IEnumerable<InputData> enumerableData = new List<InputData>();
        return mlContext.Data.LoadFromEnumerable(enumerableData);
    }
    private bool TestConn(SqlConnection conn)
    {
        conn.Open();
        if (conn.State == System.Data.ConnectionState.Open)
        {
            conn.Close();
            return true;
        }
        conn.Close();
        return false;
    }
}


/*

Error 665 = Error in SplitLogs()
Error 777 = Error in DetectSpike()
Error 3456 = Error in SaveLogs()

*/