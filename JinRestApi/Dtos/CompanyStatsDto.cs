
namespace JinRestApi.Dtos;

public class CompanyStatsDto
{
    public string? CompanyName { get; set; }
    public DateTime? LastPendingDate { get; set; }
    public int PendingCount { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int RejectedCount { get; set; }
    public double CompletionRate { get; set; }
}
