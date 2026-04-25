using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class CallManagementService
    {
        private readonly ICallManagementRepository _repository;

        public CallManagementService(ICallManagementRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CallManagement> GetAll() => _repository.GetAll();
        public CallManagement GetById(int id) => _repository.GetById(id);
        public void Add(CallManagement callManagement) => _repository.Add(callManagement);
        public void Update(CallManagement callManagement) => _repository.Update(callManagement);
        public void Delete(int id) => _repository.Delete(id);
    }
}