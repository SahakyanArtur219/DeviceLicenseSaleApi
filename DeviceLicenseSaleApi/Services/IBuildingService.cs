using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services
{
    public interface IBuildingService
    {
        IEnumerable<BuildingResponseDto> GetAll();

        BuildingResponseDto GetById(int id);

        void Create(BuildingCreateDto dto);

        void Update(int id, BuildingUpdateDto dto);

        void Delete(int id);
    }
}