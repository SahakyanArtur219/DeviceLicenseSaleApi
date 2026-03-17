namespace DeviceLicenseSaleApi.DTOs
{
    public class CreateUnifiedCommunicationsDto
    {
        public string CallRelay { get; set; }
        public string CallForwarding { get; set; }
        public string FindMeFollowMe { get; set; }
        public string Sms { get; set; }
        public string VoicemailService { get; set; }
        public string Surveillance { get; set; }
        public string UnifiedMessaging { get; set; }
    }
}