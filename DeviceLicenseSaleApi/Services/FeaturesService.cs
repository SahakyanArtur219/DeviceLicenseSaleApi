using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class FeaturesService
    {
        private readonly IFeaturesRepository _repository;

        public FeaturesService(IFeaturesRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<FeaturesDto> GetAll()
        {
            return _repository.GetAll().Select(MapToDto);
        }

        public FeaturesDto? GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : MapToDto(entity);
        }

        public FeaturesDto Add(CreateFeaturesDto dto)
        {
            var entity = new Features
            {
                AdministrativeId = dto.AdministrativeId,
                CallAnsweringId = dto.CallAnsweringId,
                CallManagementId = dto.CallManagementId,
                CallScreeningId = dto.CallScreeningId,
                GroupConvenienceId = dto.GroupConvenienceId,
                CostBandwidthSavingId = dto.CostBandwidthSavingId,
                UtilityId = dto.UtilityId,
                SecurityToolsId = dto.SecurityToolsId,
                UnifiedCommunicationsId = dto.UnifiedCommunicationsId,
                RTPStreamingChannels = dto.RTPStreamingChannels,
                HotCallAddInForMicrosoftOutlook = dto.HotCallAddInForMicrosoftOutlook,
                HotKeyCall = dto.HotKeyCall
            };

            _repository.Add(entity);
            return MapToDto(entity);
        }

        public bool Update(int id, CreateFeaturesDto dto)
        {
            var entity = _repository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            entity.AdministrativeId = dto.AdministrativeId;
            entity.CallAnsweringId = dto.CallAnsweringId;
            entity.CallManagementId = dto.CallManagementId;
            entity.CallScreeningId = dto.CallScreeningId;
            entity.GroupConvenienceId = dto.GroupConvenienceId;
            entity.CostBandwidthSavingId = dto.CostBandwidthSavingId;
            entity.UtilityId = dto.UtilityId;
            entity.SecurityToolsId = dto.SecurityToolsId;
            entity.UnifiedCommunicationsId = dto.UnifiedCommunicationsId;
            entity.RTPStreamingChannels = dto.RTPStreamingChannels;
            entity.HotCallAddInForMicrosoftOutlook = dto.HotCallAddInForMicrosoftOutlook;
            entity.HotKeyCall = dto.HotKeyCall;

            _repository.Update(entity);
            return true;
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private static FeaturesDto MapToDto(Features entity)
        {
            return new FeaturesDto
            {
                Id = entity.Id,
                AdministrativeId = entity.AdministrativeId,
                CallAnsweringId = entity.CallAnsweringId,
                CallManagementId = entity.CallManagementId,
                CallScreeningId = entity.CallScreeningId,
                GroupConvenienceId = entity.GroupConvenienceId,
                CostBandwidthSavingId = entity.CostBandwidthSavingId,
                UtilityId = entity.UtilityId,
                SecurityToolsId = entity.SecurityToolsId,
                UnifiedCommunicationsId = entity.UnifiedCommunicationsId,
                RTPStreamingChannels = entity.RTPStreamingChannels,
                HotCallAddInForMicrosoftOutlook = entity.HotCallAddInForMicrosoftOutlook,
                HotKeyCall = entity.HotKeyCall
            };
        }
    }
}
