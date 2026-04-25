using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class CostBandwidthSavingRepository : ICostBandwidthSavingRepository
    {
        private readonly AppDbContext _context;

        public CostBandwidthSavingRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CostBandwidthSaving> GetAll()
        {
            return _context.CostBandwidthSavings.ToList();
        }

        public CostBandwidthSaving GetById(int id)
        {
            return _context.CostBandwidthSavings.FirstOrDefault(c => c.Id == id);
        }

        public void Add(CostBandwidthSaving entity)
        {
            _context.CostBandwidthSavings.Add(entity);
            _context.SaveChanges();
        }

        public void Update(CostBandwidthSaving entity)
        {
            _context.CostBandwidthSavings.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.CostBandwidthSavings.FirstOrDefault(c => c.Id == id);
            if (entity != null)
            {
                _context.CostBandwidthSavings.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}