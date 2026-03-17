namespace DeviceLicenseSaleApi.Models
{
    public class CallScreening
    {
        public int Id { get; set; }
        public string CallBlocking { get; set; }
        public string DirectTransferToVoiceMailbox { get; set; }
        public string DistinctiveRinging { get; set; }
        public string DoNotDisturb { get; set; }
        public string HidingCallerId { get; set; }
    }
}