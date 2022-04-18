using System.ComponentModel.DataAnnotations;

namespace UptimeR.Domain;

public class URL
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string ServiceName { get; set; } = "";
    [Required]
    public string Url { get; set; } = "";
    public bool OnlyPing { get; set; } = false;
    [Required]
    public int Interval { get; set; } = 1;
    public DateTime LastHitTime { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0);
    public DateTime LastResultTimeOk { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0);
    public bool LastResultOk { get; set; } = false;
    public List<LogHistory> Logs { get; set; } = new List<LogHistory>();
}