using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _repository;

        public BuildingService(IBuildingRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<BuildingResponseDto> GetAll()
        {
            return _repository.GetAll().Select(b => new BuildingResponseDto
            {
                Id = b.Id,
                CompanyId = b.CompanyId,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                Country = b.Country,
                CreatedAt = b.CreatedAt
            });
        }

        public BuildingResponseDto GetById(int id)
        {
            var b = _repository.GetById(id);

            if (b == null) return null;

            return new BuildingResponseDto
            {
                Id = b.Id,
                CompanyId = b.CompanyId,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                Country = b.Country,
                CreatedAt = b.CreatedAt
            };
        }

        public void Create(BuildingCreateDto dto)
        {
            var building = new Building
            {
                CompanyId = dto.CompanyId,
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                CreatedAt = DateTime.UtcNow
            };

            _repository.Add(building);
        }

        public void Update(int id, BuildingUpdateDto dto)
        {
            var building = _repository.GetById(id);

            if (building == null) return;

            building.CompanyId = dto.CompanyId;
            building.Name = dto.Name;
            building.Address = dto.Address;
            building.City = dto.City;
            building.Country = dto.Country;

            _repository.Update(building);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}