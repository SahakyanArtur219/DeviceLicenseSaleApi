using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class CostBandwidthSavingService
    {
        private readonly ICostBandwidthSavingRepository _repository;

        public CostBandwidthSavingService(ICostBandwidthSavingRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CostBandwidthSaving> GetAll() => _repository.GetAll();
        public CostBandwidthSaving GetById(int id) => _repository.GetById(id);
        public void Add(CostBandwidthSaving entity) => _repository.Add(entity);
        public void Update(CostBandwidthSaving entity) => _repository.Update(entity);
        public void Delete(int id) => _repository.Delete(id);
    }
}