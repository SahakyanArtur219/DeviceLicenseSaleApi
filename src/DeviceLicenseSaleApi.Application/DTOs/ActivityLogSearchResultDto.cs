namespace DeviceLicenseSaleApi.DTOs
{
    public class ActivityLogSearchResultDto
    {
        public int TotalCount { get; set; }
        public int SucceededCount { get; set; }
        public int FailedCount { get; set; }
        public IReadOnlyCollection<ActivityLogItemDto> Items { get; set; } = Array.Empty<ActivityLogItemDto>();
    }
}
