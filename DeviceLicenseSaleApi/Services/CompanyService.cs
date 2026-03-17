using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CompanyResponseDto> GetAll()
        {
            return _repository.GetAll().Select(c => new CompanyResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                TIN = c.TIN,
                Address = c.Address,
                Phone = c.Phone,
                Email = c.Email,
                Website = c.Website,
                CreatedAt = c.CreatedAt
            });
        }

        public CompanyResponseDto GetById(int id)
        {
            var c = _repository.GetById(id);

            if (c == null) return null;

            return new CompanyResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                TIN = c.TIN,
                Address = c.Address,
                Phone = c.Phone,
                Email = c.Email,
                Website = c.Website,
                CreatedAt = c.CreatedAt
            };
        }

        public void Create(CompanyCreateDto dto)
        {
            var company = new Company
            {
                Name = dto.Name,
                TIN = dto.TIN,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                Website = dto.Website,
                CreatedAt = DateTime.UtcNow
            };

            _repository.Add(company);
        }

        public void Update(int id, CompanyUpdateDto dto)
        {
            var company = _repository.GetById(id);

            if (company == null) return;

            company.Name = dto.Name;
            company.Address = dto.Address;
            company.Phone = dto.Phone;
            company.Email = dto.Email;
            company.Website = dto.Website;

            _repository.Update(company);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}