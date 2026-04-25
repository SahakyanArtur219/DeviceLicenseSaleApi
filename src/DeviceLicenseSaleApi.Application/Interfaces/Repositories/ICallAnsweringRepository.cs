using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface ICallAnsweringRepository
    {
        IEnumerable<CallAnswering> GetAll();
        CallAnswering GetById(int id);
        void Add(CallAnswering callAnswering);
        void Update(CallAnswering callAnswering);
        void Delete(int id);
    }
}