using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services
{
    public interface ICompanyService
    {
        IEnumerable<CompanyResponseDto> GetAll();

        CompanyResponseDto GetById(int id);

        void Create(CompanyCreateDto dto);

        void Update(int id, CompanyUpdateDto dto);

        void Delete(int id);
    }
}