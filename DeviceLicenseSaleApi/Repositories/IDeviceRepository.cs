using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IDeviceRepository
    {
        IEnumerable<Device> GetByUserId(int userId);
        Device GetByIdForUser(int id, int userId);
        Device Add(Device device);
        void Update(Device device);
        void Delete(Device device);
    }
}