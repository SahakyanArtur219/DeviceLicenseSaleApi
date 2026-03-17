namespace DeviceLicenseSaleApi.DTOs
{
    public class CreateSecurityToolsDto
    {
        public string SystemSecuritySoftware { get; set; }
        public string Firewall { get; set; }
        public string SystemSecurityDiagnostics { get; set; }
        public string SipIds { get; set; }
        public string SecuringCallsOnAutoAttendant { get; set; }
        public string UserRightsManagement { get; set; }
        public string ClassOfService { get; set; }
        public string DateTimeSettings { get; set; }
        public string CallAlert { get; set; }
        public string SecuringCallRouting { get; set; }
        public string PinBarring { get; set; }
        public string OverallCallDurationLimit { get; set; }
    }
}