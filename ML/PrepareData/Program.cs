using Dapper;
using Mapster;
using PrepareData.Models;
using System.Data.SqlClient;

namespace PrepareData;

class Program
{
    static void Main()
    {
        var conn = new SqlConnection("Server=elonim.dyndns.dk; Database=UptimeRData; User ID=elonim; Password=Agony1-Recluse-Wham; MultipleActiveResultSets=True;");
        if (TestConn(conn))
            Work(conn);
    }

    private static bool TestConn(SqlConnection conn)
    {
        conn.Open();
        if (conn.State == System.Data.ConnectionState.Open)
        {
            Console.WriteLine("Connection successful... Proceding");

            conn.Close();
            return true;
        }
        Console.WriteLine("Connection failed... Stopping");
        conn.Close();
        return false;
    }

    private static void Work(SqlConnection conn)
    {
        var logs = conn.Query<LogHistory>("exec GetLogs");

        var adaptLogs = ConvertListToAvgLogs(logs);
        var splits = SplitLogs(adaptLogs);
        var caclulated = CalculateAvg(splits);
        Print.PrintToCSV(caclulated);
    }

    private static List<AvgLogs> ConvertListToAvgLogs(IEnumerable<LogHistory> logs)
    {
        var adaptLogs = new List<AvgLogs>();
        foreach (var log in logs)
        {
            var adaptedlog = log.Adapt<AvgLogs>();
            if (log.WasUp)
                adaptedlog.UP100 = 100;
            if (!log.WasUp)
                adaptedlog.UP100 = 0;

            adaptLogs.Add(adaptedlog);
        }

        adaptLogs.Sort((x, y) => x.Time.CompareTo(y.Time));

        return adaptLogs;
    }

    private static MultibleLogs SplitLogs(List<AvgLogs> logs)
    {
        var splittedLogs = new MultibleLogs();

        foreach (var log in logs.GroupBy(x => x.ServiceName))
        {
            splittedLogs.SortedLogs.Add(log.ToList());
        }
        return splittedLogs;
    }

    private static MultibleLogs CalculateAvg(MultibleLogs logs)
    {
        Parallel.ForEach(logs.SortedLogs, list =>
        {
            var logTime = new Queue<DateTime>();
            var logUp = new Queue<double>();
            foreach (var log in list)
            {
                logTime.Enqueue(log.Time);
                logUp.Enqueue(log.UP100);

                log.AvgUpTime = logUp.Sum() / logUp.Count();

                if (logTime.Peek() <= log.Time.AddHours(-24))
                {
                    _ = logTime.Dequeue();
                    _ = logUp.Dequeue();
                }
            }
        });
        return logs;
    }
}