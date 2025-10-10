namespace JinRestApi.Dtos
{
    public class MyCompanyStatsDto
    {
        public int PendingCount { get; set; }
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public int RejectedCount { get; set; }
        public int TotalCount { get; set; }
    }
}
