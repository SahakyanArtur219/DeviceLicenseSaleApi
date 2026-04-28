namespace DeviceLicenseSaleApi.Models
{
    public class ActivityLog
    {
        public long Id { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string Category { get; set; }
        public string Action { get; set; }
        public string Outcome { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? Description { get; set; }
        public string? TraceId { get; set; }
        public string? DetailsJson { get; set; }
    }
}
