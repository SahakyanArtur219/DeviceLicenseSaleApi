namespace DeviceLicenseSaleApi.DTOs
{
    public class SecurityToolsDto
    {
        public int Id { get; set; }
        public string SystemSecuritySoftware { get; set; }
        public string Firewall { get; set; }
        public string SecuringCallsOnAutoAttendant { get; set; }
        public string SecuringCallRouting { get; set; }
        // Expose only fields relevant for clients
    }
}