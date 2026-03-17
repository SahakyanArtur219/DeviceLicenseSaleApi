using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class UnifiedCommunicationsService
    {
        private readonly IUnifiedCommunicationsRepository _repository;

        public UnifiedCommunicationsService(IUnifiedCommunicationsRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UnifiedCommunications> GetAll() => _repository.GetAll();
        public UnifiedCommunications GetById(int id) => _repository.GetById(id);
        public void Add(UnifiedCommunications entity) => _repository.Add(entity);
        public void Update(UnifiedCommunications entity) => _repository.Update(entity);
        public void Delete(int id) => _repository.Delete(id);
    }
}