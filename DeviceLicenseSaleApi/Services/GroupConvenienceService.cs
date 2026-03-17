using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class GroupConvenienceService
    {
        private readonly IGroupConvenienceRepository _repository;

        public GroupConvenienceService(IGroupConvenienceRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<GroupConvenience> GetAll() => _repository.GetAll();
        public GroupConvenience GetById(int id) => _repository.GetById(id);
        public void Add(GroupConvenience entity) => _repository.Add(entity);
        public void Update(GroupConvenience entity) => _repository.Update(entity);
        public void Delete(int id) => _repository.Delete(id);
    }
}