using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IBuildingRepository
    {
        IEnumerable<Building> GetAll();

        Building GetById(int id);

        void Add(Building building);

        void Update(Building building);

        void Delete(int id);
    }
}