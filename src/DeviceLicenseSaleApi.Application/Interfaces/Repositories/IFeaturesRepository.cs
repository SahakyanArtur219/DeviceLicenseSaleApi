using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IFeaturesRepository
    {
        IEnumerable<Features> GetAll();
        Features GetById(int id);
        void Add(Features entity);
        void Update(Features entity);
        void Delete(int id);
    }
}