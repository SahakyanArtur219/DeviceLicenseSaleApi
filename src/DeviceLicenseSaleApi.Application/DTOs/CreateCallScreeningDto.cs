namespace DeviceLicenseSaleApi.DTOs
{
    public class CreateCallScreeningDto
    {
        public string CallBlocking { get; set; }
        public string DirectTransferToVoiceMailbox { get; set; }
        public string DistinctiveRinging { get; set; }
        public string DoNotDisturb { get; set; }
        public string HidingCallerId { get; set; }
    }
}