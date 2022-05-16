using System.Data.SqlClient;
using Dapper;
using Mapster;
using Microsoft.ML;
using UptimeR.ML.Domain.Models;
using UptimeR.ML.Domain.Models.MLModels;
using UptimeR.ML.Domain.Models.RavenModels;
using UptimeR.ML.Trainer.Interfaces;

namespace UptimeR.ML.Trainer;

public class AnomalyDetector : IAnomalyDetector
{
    private readonly IRavenDB _ravenDB;
    private readonly ISQLConn _sqlConn;
    public AnomalyDetector(IRavenDB ravenDB, ISQLConn sqlConn)
    {
        _ravenDB = ravenDB;
        _sqlConn = sqlConn;
    }

    public void Detect()
    {
        var query = "select Time,Latency,ServiceName from LogHistorys";
        var conn = _sqlConn.CreateConnection();
        if (_sqlConn.TestConn(conn))
            Work(conn, query);
    }

    public void Detect24Hours(DateOnly time)
    {
        var fromDate = time.ToString("yyyy-MM-dd");
        var toDate = time.AddDays(1).ToString("yyyy-MM-dd");

        var query = $"exec GetLogsBetween '{fromDate}', '{toDate}'";
        var conn = _sqlConn.CreateConnection();
        if (_sqlConn.TestConn(conn))
            Work(conn, query);
    }

    private void Work(SqlConnection conn, string query)
    {
        var allLogs = conn.Query<Logs>(query);
        var sortedLogs = new SortedLogs().SplitLogs(allLogs);
        Calculate(sortedLogs);
    }

    private void Calculate(SortedLogs sortedLogs)
    {
        var serviceAnomalies = new ServiceAnomalies(); //opret objekt for at vise resultater af anomaliteter
        //serviceAnomalies.Date = null;


        Parallel.ForEach(sortedLogs.Logs, list =>
        {
            var logdata = list.Adapt<List<InputData>>();
            var mlContext = new MLContext();
            IDataView dataView = mlContext.Data.LoadFromEnumerable<InputData>(logdata);
            var predictions = DetectSpike(mlContext, list.Count(), dataView);
            var ravenlog = new RavenLog();
            try
            {
                ravenlog.ServiceName = list[0].ServiceName;
                ravenlog.Date = DateOnly.FromDateTime(list[0].Time);
                foreach (var p in predictions)
                {
                    ravenlog.Logs.Add(new AnomalyLog
                    {
                        Alert = p.Prediction[0],
                        Score = p.Prediction[1],
                        PValue = p.Prediction[2],
                        Time = p.Time
                    });
                }
                ravenlog.NumberOfAnomalies = CountAnomalies(ravenlog.Logs);
            }
            catch (Exception)
            {
                throw new Exception("Convert Error");
            }

            try
            {
                //opret objekt for at vise resultater af anomaliteter
                var anomaly = new Anomaly();
                anomaly.Servicename = list[0].ServiceName;
                anomaly.AnomalyCount = ravenlog.NumberOfAnomalies;
                serviceAnomalies.Anomalies.Add(anomaly);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            serviceAnomalies.Date = DateOnly.FromDateTime(list[0].Time);//burde ikke stå her da den bliver kørt i hvert loop og ikke kun en gang
            _ravenDB.Save(ravenlog);
        });


        serviceAnomalies.Anomalies.Sort((x, y) => x.Servicename.CompareTo(y.Servicename));
        _ravenDB.Save(serviceAnomalies);
    }

    private IEnumerable<AnomalyPrediction> DetectSpike(MLContext mlContext, int docSize, IDataView dataview)
    {
        try
        {
            var iidSpikeEstimator = mlContext.Transforms.DetectIidSpike(outputColumnName: nameof(AnomalyPrediction.Prediction),
                                    inputColumnName: nameof(InputData.Latency),
                                    confidence: 95.0,
                                    pvalueHistoryLength: docSize / 4);

            ITransformer iidSpikeTransform = iidSpikeEstimator.Fit(CreateEmptyDataView(mlContext));
            IDataView transformedData = iidSpikeTransform.Transform(dataview);
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

    private int CountAnomalies(List<AnomalyLog> logs)
    {
        var anoCount = 0;

        var anomality = 0;
        foreach (var log in logs)
        {
            if (log.Alert == 1)
            {
                anomality++;
            }
            if (anomality == 2)
            {
                anoCount++;
            }
            if (log.Alert == 0)
            {
                anomality = 0;
            }
        }
        return anoCount;
    }
}


/*

Error 665 = Error in SplitLogs()
Error 777 = Error in DetectSpike()
Error 3456 = Error in SaveLogs()

*/