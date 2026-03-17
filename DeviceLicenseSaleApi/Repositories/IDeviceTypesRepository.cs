using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IDeviceTypesRepository
    {
        IEnumerable<DeviceTypes> GetAll();
        DeviceTypes GetById(int id);
        void Add(DeviceTypes entity);
        void Update(DeviceTypes entity);
        void Delete(int id);
    }
}