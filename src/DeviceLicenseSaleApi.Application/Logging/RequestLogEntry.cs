namespace DeviceLicenseSaleApi.Logging
{
    public class RequestLogEntry
    {
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string? QueryString { get; set; }
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public string? RemoteIpAddress { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? TraceId { get; set; }
    }
}
