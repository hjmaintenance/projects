namespace JinRestApi.Dtos
{
    public class AllAdminStatsDto
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string? AdminPhoto { get; set; }
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public int RejectedCount { get; set; }
        public double AcceptanceRate { get; set; }
        public double CompletionRate { get; set; }
        public int TotalHandled { get; set; }
    }
}
