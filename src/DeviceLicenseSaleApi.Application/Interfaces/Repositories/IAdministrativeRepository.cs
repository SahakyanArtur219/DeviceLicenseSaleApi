using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IAdministrativeRepository
    {
        IEnumerable<Administrative> GetAll();
        Administrative GetById(int id);
        void Add(Administrative admin);
        void Update(Administrative admin);
        void Delete(int id);
    }
}