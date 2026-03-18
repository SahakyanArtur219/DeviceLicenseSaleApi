using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services
{
    public interface IDeviceService
    {
        IEnumerable<DeviceResponseDto> GetByUserId(int userId);
        DeviceResponseDto GetByIdForUser(int id, int userId);
        DeviceResponseDto CreateForUser(int userId, DeviceCreateDto dto);
        bool UpdateForUser(int id, int userId, DeviceUpdateDto dto);
        bool DeleteForUser(int id, int userId);
    }
}