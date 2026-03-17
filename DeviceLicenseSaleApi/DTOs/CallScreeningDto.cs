namespace DeviceLicenseSaleApi.DTOs
{
    public class CallScreeningDto
    {
        public int Id { get; set; }
        public string CallBlocking { get; set; }
        public string DoNotDisturb { get; set; }
        public string HidingCallerId { get; set; }
        // Expose only fields relevant for clients
    }
}