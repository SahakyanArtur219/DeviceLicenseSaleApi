using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ILicenseRepository _repository;

        public LicenseService(ILicenseRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<LicenseResponseDto> GetAll()
        {
            return _repository.GetAll().Select(MapToResponse);
        }

        public LicenseResponseDto GetById(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return null;

            return MapToResponse(entity);
        }

        public LicenseResponseDto Create(LicenseCreateDto dto)
        {
            if (_repository.ExistsByLicenseKey(dto.LicenseKey))
                throw new Exception("License key already exists");

            var entity = new License
            {
                LicenseKey = dto.LicenseKey,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = dto.ExpiresAt,
                IsActive = dto.IsActive ?? true,
                IPPhoneExpansionKey = dto.IPPhoneExpansionKey,
                ConcurrentCallExpansion = dto.ConcurrentCallExpansion,
                CallRecording = dto.CallRecording,
                CallingCostControl = dto.CallingCostControl,
                AudioConferenceBridge = dto.AudioConferenceBridge,
                VideoConferenceBridge = dto.VideoConferenceBridge,
                ThirdPartyCallControl3PCC = dto.ThirdPartyCallControl3PCC,
                AdvancedProxyConnectionService = dto.AdvancedProxyConnectionService,
                eQallSoftphone = dto.eQallSoftphone,
                eQallReceptionistConsole = dto.eQallReceptionistConsole,
                eQallSMSWhatsAppMessaging = dto.eQallSMSWhatsAppMessaging,
                CRMIntegration = dto.CRMIntegration,
                VoiceMailCallRecordingTranscription = dto.VoiceMailCallRecordingTranscription,
                TextToSpeechTranscription = dto.TextToSpeechTranscription,
                VoiceEnabledAutoAttendant = dto.VoiceEnabledAutoAttendant,
                AutomaticCallDistributionACD = dto.AutomaticCallDistributionACD,
                EpygiACDConsoleEAC = dto.EpygiACDConsoleEAC,
                AutomaticOutboundCallingAOC = dto.AutomaticOutboundCallingAOC,
                BargeIn = dto.BargeIn,
                AutoDialerActivation = dto.AutoDialerActivation,
                AutoDialerExpansionKey = dto.AutoDialerExpansionKey,
                PCCActivationLicense = dto.PCCActivationLicense,
                ServerSystemRedundancyActivation = dto.ServerSystemRedundancyActivation
            };

            var created = _repository.Add(entity);
            return MapToResponse(created);
        }

        public void Update(int id, LicenseUpdateDto dto)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return;

            entity.LicenseKey = dto.LicenseKey;
            entity.ExpiresAt = dto.ExpiresAt;
            entity.IsActive = dto.IsActive;
            entity.IPPhoneExpansionKey = dto.IPPhoneExpansionKey;
            entity.ConcurrentCallExpansion = dto.ConcurrentCallExpansion;
            entity.CallRecording = dto.CallRecording;
            entity.CallingCostControl = dto.CallingCostControl;
            entity.AudioConferenceBridge = dto.AudioConferenceBridge;
            entity.VideoConferenceBridge = dto.VideoConferenceBridge;
            entity.ThirdPartyCallControl3PCC = dto.ThirdPartyCallControl3PCC;
            entity.AdvancedProxyConnectionService = dto.AdvancedProxyConnectionService;
            entity.eQallSoftphone = dto.eQallSoftphone;
            entity.eQallReceptionistConsole = dto.eQallReceptionistConsole;
            entity.eQallSMSWhatsAppMessaging = dto.eQallSMSWhatsAppMessaging;
            entity.CRMIntegration = dto.CRMIntegration;
            entity.VoiceMailCallRecordingTranscription = dto.VoiceMailCallRecordingTranscription;
            entity.TextToSpeechTranscription = dto.TextToSpeechTranscription;
            entity.VoiceEnabledAutoAttendant = dto.VoiceEnabledAutoAttendant;
            entity.AutomaticCallDistributionACD = dto.AutomaticCallDistributionACD;
            entity.EpygiACDConsoleEAC = dto.EpygiACDConsoleEAC;
            entity.AutomaticOutboundCallingAOC = dto.AutomaticOutboundCallingAOC;
            entity.BargeIn = dto.BargeIn;
            entity.AutoDialerActivation = dto.AutoDialerActivation;
            entity.AutoDialerExpansionKey = dto.AutoDialerExpansionKey;
            entity.PCCActivationLicense = dto.PCCActivationLicense;
            entity.ServerSystemRedundancyActivation = dto.ServerSystemRedundancyActivation;

            _repository.Update(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private static LicenseResponseDto MapToResponse(License x)
        {
            return new LicenseResponseDto
            {
                Id = x.Id,
                LicenseKey = x.LicenseKey,
                CreatedAt = x.CreatedAt,
                ExpiresAt = x.ExpiresAt,
                IsActive = x.IsActive,
                IPPhoneExpansionKey = x.IPPhoneExpansionKey,
                ConcurrentCallExpansion = x.ConcurrentCallExpansion,
                CallRecording = x.CallRecording,
                CallingCostControl = x.CallingCostControl,
                AudioConferenceBridge = x.AudioConferenceBridge,
                VideoConferenceBridge = x.VideoConferenceBridge,
                ThirdPartyCallControl3PCC = x.ThirdPartyCallControl3PCC,
                AdvancedProxyConnectionService = x.AdvancedProxyConnectionService,
                eQallSoftphone = x.eQallSoftphone,
                eQallReceptionistConsole = x.eQallReceptionistConsole,
                eQallSMSWhatsAppMessaging = x.eQallSMSWhatsAppMessaging,
                CRMIntegration = x.CRMIntegration,
                VoiceMailCallRecordingTranscription = x.VoiceMailCallRecordingTranscription,
                TextToSpeechTranscription = x.TextToSpeechTranscription,
                VoiceEnabledAutoAttendant = x.VoiceEnabledAutoAttendant,
                AutomaticCallDistributionACD = x.AutomaticCallDistributionACD,
                EpygiACDConsoleEAC = x.EpygiACDConsoleEAC,
                AutomaticOutboundCallingAOC = x.AutomaticOutboundCallingAOC,
                BargeIn = x.BargeIn,
                AutoDialerActivation = x.AutoDialerActivation,
                AutoDialerExpansionKey = x.AutoDialerExpansionKey,
                PCCActivationLicense = x.PCCActivationLicense,
                ServerSystemRedundancyActivation = x.ServerSystemRedundancyActivation
            };
        }
    }
}