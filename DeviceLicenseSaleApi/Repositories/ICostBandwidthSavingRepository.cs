using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface ICostBandwidthSavingRepository
    {
        IEnumerable<CostBandwidthSaving> GetAll();
        CostBandwidthSaving GetById(int id);
        void Add(CostBandwidthSaving entity);
        void Update(CostBandwidthSaving entity);
        void Delete(int id);
    }
}