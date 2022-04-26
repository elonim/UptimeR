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
        if(TestConn(conn))
            Work(conn);
    }

    private static bool TestConn(SqlConnection conn)
    {
        conn.Open();
        if(conn.State == System.Data.ConnectionState.Open)
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
        var urls = conn.Query<URL>("exec GetServices");
        var logs = conn.Query<LogHistory>("exec GetLogs");

        var adaptLog = SortAndConvert(logs);
        var caclulatedAvg = CalculateAvg(urls, adaptLog);

        Print.PrintToCSV(caclulatedAvg);
    }

    private static List<avgLogs> SortAndConvert(IEnumerable<LogHistory> logs)
    {
        var adaptLog = logs.Adapt<List<avgLogs>>();
        foreach(var log in adaptLog)
        {
            if(log.WasUp)
                log.UP100 = 100;
            if(!log.WasUp)
                log.UP100 = 0;
        }
        adaptLog.Sort((x, y) => x.Time.CompareTo(y.Time));
        return adaptLog;
    }

    private static List<avgLogs> CalculateAvg(IEnumerable<URL> urls, List<avgLogs> adaptLog)
    {
        var TimeKeeper = DateTime.Now;
        var avgList = new List<double>();

        foreach(var urlItem in urls)
        {
            TimeKeeper = new DateTime(1900, 1, 1, 0, 0, 0);

            foreach(var log in adaptLog.Where(x => x.URLId == urlItem.Id))
            {
                if(log.Time >= TimeKeeper)
                {
                    //reset day
                    TimeKeeper = log.Time.AddDays(1);
                    avgList = new List<double>();
                }
                if(log.Time < TimeKeeper)
                {
                    avgList.Add(log.UP100);
                    log.UpAvg24Hrs = avgList.Average();
                }
            }
        }
        return adaptLog;
    }
}