
namespace UptimeR.DTOs;

public class URLDTO
{
    public Guid Id { get; set; }
    public string ServiceName { get; set; } = "";
    public string Url { get; set; } = "";
    public bool OnlyPing { get; set; } = false;
    public DateTime LastResultTimeOk { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0);
    public bool LastResultOk { get; set; } = false;
}
