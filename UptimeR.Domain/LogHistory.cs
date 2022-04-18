using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UptimeR.Domain;

public class LogHistory
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string ServiceName { get; set; } = "Unknown";
    [Required]
    [ForeignKey("URL")]
    public Guid URLId { get; set; }
    [Required]
    public DateTime Time { get; set; }
    public bool UsedPing { get; set; }
    public bool UsedHttp { get; set; }
    public double Latency { get; set; }
    public bool WasUp { get; set; }

}