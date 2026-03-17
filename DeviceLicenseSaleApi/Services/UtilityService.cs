using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class UtilityService
    {
        private readonly IUtilityRepository _repository;

        public UtilityService(IUtilityRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Utility> GetAll() => _repository.GetAll();
        public Utility GetById(int id) => _repository.GetById(id);
        public void Add(Utility entity) => _repository.Add(entity);
        public void Update(Utility entity) => _repository.Update(entity);
        public void Delete(int id) => _repository.Delete(id);
    }
}