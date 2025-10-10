namespace JinRestApi.Dtos
{
    public class AdminStatsDto
    {
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public int RejectedCount { get; set; }
        public int TotalRequests { get; set; }
    }
}
