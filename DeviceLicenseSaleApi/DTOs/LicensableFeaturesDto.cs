namespace DeviceLicenseSaleApi.DTOs
{
    public class LicensableFeaturesDto
    {
        public int Id { get; set; }
        public int? IPPhoneExpansionKey { get; set; }
        public int? ConcurrentCallExpansion { get; set; }
        public int? CallRecording { get; set; }
        public bool? CallingCostControl { get; set; }
        public int? AudioConferenceBridge { get; set; }
        public int? VideoConferenceBridge { get; set; }
        public bool? CRMIntegration { get; set; }
    }
}