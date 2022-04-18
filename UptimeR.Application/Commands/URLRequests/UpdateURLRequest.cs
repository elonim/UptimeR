namespace UptimeR.Application.Commands.URLRequests;

public class UpdateURLRequest
{
    public Guid Id { get; set; }
    public string ServiceName { get; set; } = "";
    public string Url { get; set; } = "";
    public bool OnlyPing { get; set; } = false;
    public int Interval { get; set; } = 1;
    public DateTime LastHitTime { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0);
    public DateTime LastResultTimeOk { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0);
    public bool LastResultOk { get; set; } = false;
}
