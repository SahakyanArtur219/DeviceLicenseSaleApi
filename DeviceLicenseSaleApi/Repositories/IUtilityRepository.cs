using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IUtilityRepository
    {
        IEnumerable<Utility> GetAll();
        Utility GetById(int id);
        void Add(Utility entity);
        void Update(Utility entity);
        void Delete(int id);
    }
}