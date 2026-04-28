namespace DeviceLicenseSaleApi.Models
{
    public class RequestLog
    {
        public long Id { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string? QueryString { get; set; }
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public string? RemoteIpAddress { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? TraceId { get; set; }
    }
}
