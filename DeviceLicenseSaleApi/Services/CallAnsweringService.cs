using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class CallAnsweringService
    {
        private readonly ICallAnsweringRepository _repository;

        public CallAnsweringService(ICallAnsweringRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CallAnswering> GetAll() => _repository.GetAll();
        public CallAnswering GetById(int id) => _repository.GetById(id);
        public void Add(CallAnswering callAnswering) => _repository.Add(callAnswering);
        public void Update(CallAnswering callAnswering) => _repository.Update(callAnswering);
        public void Delete(int id) => _repository.Delete(id);
    }
}