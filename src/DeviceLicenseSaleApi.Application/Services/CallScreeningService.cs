using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class CallScreeningService
    {
        private readonly ICallScreeningRepository _repository;

        public CallScreeningService(ICallScreeningRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CallScreening> GetAll() => _repository.GetAll();
        public CallScreening GetById(int id) => _repository.GetById(id);
        public void Add(CallScreening callScreening) => _repository.Add(callScreening);
        public void Update(CallScreening callScreening) => _repository.Update(callScreening);
        public void Delete(int id) => _repository.Delete(id);
    }
}