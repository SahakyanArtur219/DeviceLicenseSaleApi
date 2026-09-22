using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services;

public interface IWeeklyBundleService
{
    IEnumerable<WeeklyBundleResponseDto> GetAll();
    IEnumerable<WeeklyBundleResponseDto> GetActiveForDeviceType(int deviceTypeId);
    WeeklyBundleResponseDto Create(WeeklyBundleCreateDto dto);
    void Delete(int id);
}