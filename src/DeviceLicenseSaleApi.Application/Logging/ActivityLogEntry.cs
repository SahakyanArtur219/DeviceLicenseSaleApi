namespace DeviceLicenseSaleApi.Logging
{
    public class ActivityLogEntry
    {
        public string Category { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Outcome { get; set; } = "Succeeded";
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? Description { get; set; }
        public string? TraceId { get; set; }
        public IDictionary<string, object?>? Details { get; set; }
    }
}
