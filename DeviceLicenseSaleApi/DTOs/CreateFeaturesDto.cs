namespace DeviceLicenseSaleApi.DTOs
{
    public class CreateFeaturesDto
    {
        public int AdministrativeId { get; set; }
        public int CallAnsweringId { get; set; }
        public int CallManagementId { get; set; }
        public int CallScreeningId { get; set; }
        public int GroupConvenienceId { get; set; }
        public int CostBandwidthSavingId { get; set; }
        public int UtilityId { get; set; }
        public int SecurityToolsId { get; set; }
        public int UnifiedCommunicationsId { get; set; }

        public string RTPStreamingChannels { get; set; }
        public string HotCallAddInForMicrosoftOutlook { get; set; }
        public string HotKeyCall { get; set; }
    }
}