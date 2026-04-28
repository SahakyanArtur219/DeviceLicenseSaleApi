namespace DeviceLicenseSaleApi.DTOs
{
    public class ActivityLogItemDto
    {
        public long Id { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Outcome { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? Description { get; set; }
        public string? TraceId { get; set; }
        public string? DetailsJson { get; set; }
    }
}
