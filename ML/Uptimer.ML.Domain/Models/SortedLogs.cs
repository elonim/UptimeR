
namespace UptimeR.ML.Domain.Models;

public class SortedLogs
{
    public List<List<Logs>> Logs { get; set; } = new();

    public SortedLogs SplitLogs(IEnumerable<Logs> logs)
    {
        try
        {
            foreach (var log in logs.GroupBy(x => x.ServiceName))
            {
                this.Logs.Add(log.ToList());
            }
            return this;
        }
        catch (Exception)
        {
            throw new Exception("Error 665");
        }
    }
}