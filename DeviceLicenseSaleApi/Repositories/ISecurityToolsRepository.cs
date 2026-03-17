using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface ISecurityToolsRepository
    {
        IEnumerable<SecurityTools> GetAll();
        SecurityTools GetById(int id);
        void Add(SecurityTools entity);
        void Update(SecurityTools entity);
        void Delete(int id);
    }
}