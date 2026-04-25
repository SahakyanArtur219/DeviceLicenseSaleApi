using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class AdministrativeService
    {
        private readonly IAdministrativeRepository _repository;

        public AdministrativeService(IAdministrativeRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Administrative> GetAll() => _repository.GetAll();
        public Administrative GetById(int id) => _repository.GetById(id);
        public void Add(Administrative admin) => _repository.Add(admin);
        public void Update(Administrative admin) => _repository.Update(admin);
        public void Delete(int id) => _repository.Delete(id);
    }
}