using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class LicensableFeaturesRepository : ILicensableFeaturesRepository
    {
        private readonly AppDbContext _context;

        public LicensableFeaturesRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LicensableFeatures> GetAll()
        {
            return _context.LicensableFeatures.ToList();
        }

        public LicensableFeatures GetById(int id)
        {
            return _context.LicensableFeatures.FirstOrDefault(l => l.Id == id);
        }

        public void Add(LicensableFeatures entity)
        {
            _context.LicensableFeatures.Add(entity);
            _context.SaveChanges();
        }

        public void Update(LicensableFeatures entity)
        {
            _context.LicensableFeatures.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.LicensableFeatures.FirstOrDefault(l => l.Id == id);
            if (entity != null)
            {
                _context.LicensableFeatures.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}