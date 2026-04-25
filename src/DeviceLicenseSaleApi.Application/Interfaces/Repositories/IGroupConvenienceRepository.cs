using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IGroupConvenienceRepository
    {
        IEnumerable<GroupConvenience> GetAll();
        GroupConvenience GetById(int id);
        void Add(GroupConvenience entity);
        void Update(GroupConvenience entity);
        void Delete(int id);
    }
}