using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class DeviceTypesService
    {
        private readonly IDeviceTypesRepository _repository;

        public DeviceTypesService(IDeviceTypesRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<DeviceTypesDto> GetAll()
        {
            return _repository.GetAll().Select(MapToDto);
        }

        public DeviceTypesDto? GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : MapToDto(entity);
        }

        public DeviceTypesDto Add(CreateDeviceTypesDto dto)
        {
            var entity = new DeviceTypes
            {
                Name = dto.Name,
                AnalogPhones = dto.AnalogPhones,
                IPPhones = dto.IPPhones,
                AdditionalIPPhonesWithKeys = dto.AdditionalIPPhonesWithKeys,
                TotalPhones = dto.TotalPhones,
                ConcurrentCalls = dto.ConcurrentCalls,
                AdditionalConcurrentCallsWithKeys = dto.AdditionalConcurrentCallsWithKeys,
                EthernetLANPorts = dto.EthernetLANPorts,
                EthernetWANPorts = dto.EthernetWANPorts,
                AudioInPorts = dto.AudioInPorts,
                AudioOutPorts = dto.AudioOutPorts,
                SDSlots = dto.SDSlots,
                FeatureId = dto.FeatureId,
                LicensableFeatureId = dto.LicensableFeatureId
            };

            _repository.Add(entity);
            return MapToDto(entity);
        }

        public bool Update(int id, CreateDeviceTypesDto dto)
        {
            var entity = _repository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            entity.Name = dto.Name;
            entity.AnalogPhones = dto.AnalogPhones;
            entity.IPPhones = dto.IPPhones;
            entity.AdditionalIPPhonesWithKeys = dto.AdditionalIPPhonesWithKeys;
            entity.TotalPhones = dto.TotalPhones;
            entity.ConcurrentCalls = dto.ConcurrentCalls;
            entity.AdditionalConcurrentCallsWithKeys = dto.AdditionalConcurrentCallsWithKeys;
            entity.EthernetLANPorts = dto.EthernetLANPorts;
            entity.EthernetWANPorts = dto.EthernetWANPorts;
            entity.AudioInPorts = dto.AudioInPorts;
            entity.AudioOutPorts = dto.AudioOutPorts;
            entity.SDSlots = dto.SDSlots;
            entity.FeatureId = dto.FeatureId;
            entity.LicensableFeatureId = dto.LicensableFeatureId;

            _repository.Update(entity);
            return true;
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private static DeviceTypesDto MapToDto(DeviceTypes entity)
        {
            return new DeviceTypesDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AnalogPhones = entity.AnalogPhones,
                IPPhones = entity.IPPhones,
                AdditionalIPPhonesWithKeys = entity.AdditionalIPPhonesWithKeys,
                TotalPhones = entity.TotalPhones,
                ConcurrentCalls = entity.ConcurrentCalls,
                AdditionalConcurrentCallsWithKeys = entity.AdditionalConcurrentCallsWithKeys,
                EthernetLANPorts = entity.EthernetLANPorts,
                EthernetWANPorts = entity.EthernetWANPorts,
                AudioInPorts = entity.AudioInPorts,
                AudioOutPorts = entity.AudioOutPorts,
                SDSlots = entity.SDSlots,
                FeatureId = entity.FeatureId,
                LicensableFeatureId = entity.LicensableFeatureId
            };
        }
    }
}
