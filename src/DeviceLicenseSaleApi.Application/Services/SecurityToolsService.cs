using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class SecurityToolsService
    {
        private readonly ISecurityToolsRepository _repository;

        public SecurityToolsService(ISecurityToolsRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<SecurityTools> GetAll() => _repository.GetAll();
        public SecurityTools GetById(int id) => _repository.GetById(id);
        public void Add(SecurityTools entity) => _repository.Add(entity);
        public void Update(SecurityTools entity) => _repository.Update(entity);
        public void Delete(int id) => _repository.Delete(id);
    }
}