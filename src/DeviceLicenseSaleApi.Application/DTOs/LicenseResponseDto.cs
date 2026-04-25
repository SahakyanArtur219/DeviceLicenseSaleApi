namespace DeviceLicenseSaleApi.DTOs
{
    public class LicenseResponseDto
    {
        public int Id { get; set; }

        public string LicenseKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }

        public int? IPPhoneExpansionKey { get; set; }
        public int? ConcurrentCallExpansion { get; set; }
        public int? CallRecording { get; set; }
        public bool? CallingCostControl { get; set; }
        public int? AudioConferenceBridge { get; set; }
        public int? VideoConferenceBridge { get; set; }
        public bool? ThirdPartyCallControl3PCC { get; set; }
        public bool? AdvancedProxyConnectionService { get; set; }
        public int? eQallSoftphone { get; set; }
        public int? eQallReceptionistConsole { get; set; }
        public int? eQallSMSWhatsAppMessaging { get; set; }
        public bool? CRMIntegration { get; set; }
        public int? VoiceMailCallRecordingTranscription { get; set; }
        public bool? TextToSpeechTranscription { get; set; }
        public bool? VoiceEnabledAutoAttendant { get; set; }
        public bool? AutomaticCallDistributionACD { get; set; }
        public int? EpygiACDConsoleEAC { get; set; }
        public bool? AutomaticOutboundCallingAOC { get; set; }
        public bool? BargeIn { get; set; }
        public bool? AutoDialerActivation { get; set; }
        public int? AutoDialerExpansionKey { get; set; }
        public bool? PCCActivationLicense { get; set; }
        public bool? ServerSystemRedundancyActivation { get; set; }
    }
}