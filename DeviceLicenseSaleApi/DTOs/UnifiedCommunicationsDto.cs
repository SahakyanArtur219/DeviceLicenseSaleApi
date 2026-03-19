namespace DeviceLicenseSaleApi.DTOs
{
    public class UnifiedCommunicationsDto
    {
        public int Id { get; set; }
        public string CallRelay { get; set; }
        public string CallForwarding { get; set; }
        public string Sms { get; set; }
        public string VoicemailService { get; set; }
    }
}