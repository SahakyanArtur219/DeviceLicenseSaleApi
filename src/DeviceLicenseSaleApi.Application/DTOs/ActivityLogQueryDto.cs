namespace DeviceLicenseSaleApi.DTOs
{
    public class ActivityLogQueryDto
    {
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public string? Category { get; set; }
        public string? Action { get; set; }
        public string? Outcome { get; set; }
        public int? UserId { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
