using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface ICallManagementRepository
    {
        IEnumerable<CallManagement> GetAll();
        CallManagement GetById(int id);
        void Add(CallManagement callManagement);
        void Update(CallManagement callManagement);
        void Delete(int id);
    }
}