using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface ICallScreeningRepository
    {
        IEnumerable<CallScreening> GetAll();
        CallScreening GetById(int id);
        void Add(CallScreening callScreening);
        void Update(CallScreening callScreening);
        void Delete(int id);
    }
}