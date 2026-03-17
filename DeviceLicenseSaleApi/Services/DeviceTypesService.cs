using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class DeviceTypesService
    {
        private readonly IDeviceTypesRepository _repository;

        public DeviceTypesService(IDeviceTypesRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<DeviceTypes> GetAll() => _repository.GetAll();
        public DeviceTypes GetById(int id) => _repository.GetById(id);
        public void Add(DeviceTypes entity) => _repository.Add(entity);
        public void Update(DeviceTypes entity) => _repository.Update(entity);
        public void Delete(int id) => _repository.Delete(id);
    }
}