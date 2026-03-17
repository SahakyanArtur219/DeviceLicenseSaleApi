using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IUnifiedCommunicationsRepository
    {
        IEnumerable<UnifiedCommunications> GetAll();
        UnifiedCommunications GetById(int id);
        void Add(UnifiedCommunications entity);
        void Update(UnifiedCommunications entity);
        void Delete(int id);
    }
}