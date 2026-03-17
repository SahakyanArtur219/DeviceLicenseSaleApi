using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceLicenseSaleApi.Models
{
    public class Feature
    {
        public int Id { get; set; }

        // Foreign Keys
        public int AdministrativeId { get; set; }
        public int CallAnsweringId { get; set; }
        public int CallManagementId { get; set; }
        public int CallScreeningId { get; set; }
        public int GroupConvenienceId { get; set; }
        public int CostBandwidthSavingId { get; set; }
        public int UtilityId { get; set; }
        public int SecurityToolsId { get; set; }
        public int UnifiedCommunicationsId { get; set; }

        // Navigation Properties
        [ForeignKey("AdministrativeId")]
        public Administrative Administrative { get; set; }

        [ForeignKey("CallAnsweringId")]
        public CallAnswering CallAnswering { get; set; }

        [ForeignKey("CallManagementId")]
        public CallManagement CallManagement { get; set; }

        [ForeignKey("CallScreeningId")]
        public CallScreening CallScreening { get; set; }

        [ForeignKey("GroupConvenienceId")]
        public GroupConvenience GroupConvenience { get; set; }

        [ForeignKey("CostBandwidthSavingId")]
        public CostBandwidthSaving CostBandwidthSaving { get; set; }

        [ForeignKey("UtilityId")]
        public Utility Utility { get; set; }

        [ForeignKey("SecurityToolsId")]
        public SecurityTool SecurityTools { get; set; }

        [ForeignKey("UnifiedCommunicationsId")]
        public UnifiedCommunication UnifiedCommunications { get; set; }

        // Extra fields
        public string RTPStreamingChannels { get; set; }
        public string HotCallAddInForMicrosoftOutlook { get; set; }
        public string HotKeyCall { get; set; }
    }
}