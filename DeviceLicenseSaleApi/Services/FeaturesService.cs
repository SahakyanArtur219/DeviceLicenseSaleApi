using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class FeaturesService
    {
        private readonly IFeaturesRepository _repository;

        public FeaturesService(IFeaturesRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Features> GetAll() => _repository.GetAll();
        public Features GetById(int id) => _repository.GetById(id);
        public void Add(Features entity) => _repository.Add(entity);
        public void Update(Features entity) => _repository.Update(entity);
        public void Delete(int id) => _repository.Delete(id);
    }
}