using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services
{
    public interface ILicenseService
    {
        IEnumerable<LicenseResponseDto> GetAll();
        LicenseResponseDto GetById(int id);
        LicenseResponseDto Create(LicenseCreateDto dto);
        void Update(int id, LicenseUpdateDto dto);
        void Delete(int id);
    }
}