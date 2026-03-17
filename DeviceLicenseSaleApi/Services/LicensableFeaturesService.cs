using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class LicensableFeaturesService
    {
        private readonly ILicensableFeaturesRepository _repository;

        public LicensableFeaturesService(ILicensableFeaturesRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<LicensableFeatures> GetAll() => _repository.GetAll();
        public LicensableFeatures GetById(int id) => _repository.GetById(id);
        public void Add(LicensableFeatures entity) => _repository.Add(entity);
        public void Update(LicensableFeatures entity) => _repository.Update(entity);
        public void Delete(int id) => _repository.Delete(id);
    }
}