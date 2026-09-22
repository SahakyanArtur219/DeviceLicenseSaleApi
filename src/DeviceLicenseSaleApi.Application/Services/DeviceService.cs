using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _repository;

        public DeviceService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<DeviceResponseDto> GetByUserId(int userId)
        {
            return _repository.GetByUserId(userId).Select(Map);
        }

        public DeviceResponseDto GetByIdForUser(int id, int userId)
        {
            var entity = _repository.GetByIdForUser(id, userId);
            return entity == null ? null : Map(entity);
        }

        public DeviceResponseDto CreateForUser(int userId, DeviceCreateDto dto)
        {
            var entity = new Device
            {
                UserId = userId,
                DeviceTypeId = dto.DeviceTypeId,
                LicenseId = dto.LicenseId,
                Name = dto.Name,
                Location = dto.Location,
                SerialNumber = string.Empty
            };

            var created = _repository.Add(entity);
            if (string.IsNullOrWhiteSpace(created.SerialNumber))
            {
                created.SerialNumber = $"DEV-{created.Id:000000}";
                _repository.Update(created);
            }

            return Map(created);
        }

        public bool UpdateForUser(int id, int userId, DeviceUpdateDto dto)
        {
            var entity = _repository.GetByIdForUser(id, userId);
            if (entity == null) return false;

            entity.DeviceTypeId = dto.DeviceTypeId;
            entity.LicenseId = dto.LicenseId;
            entity.Name = dto.Name;
            entity.Location = dto.Location;

            _repository.Update(entity);
            return true;
        }

        public bool DeleteForUser(int id, int userId)
        {
            var entity = _repository.GetByIdForUser(id, userId);
            if (entity == null) return false;

            _repository.Delete(entity);
            return true;
        }

        private static DeviceResponseDto Map(Device x)
        {
            return new DeviceResponseDto
            {
                Id = x.Id,
                UserId = x.UserId,
                DeviceTypeId = x.DeviceTypeId,
                LicenseId = x.LicenseId,
                Name = x.Name,
                SerialNumber = x.SerialNumber,
                Location = x.Location
            };
        }
    }
}