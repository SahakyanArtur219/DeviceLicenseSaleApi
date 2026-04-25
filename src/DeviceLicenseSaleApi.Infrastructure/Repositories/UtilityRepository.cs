using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class UtilityRepository : IUtilityRepository
    {
        private readonly AppDbContext _context;

        public UtilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Utility> GetAll()
        {
            return _context.Utilities.ToList();
        }

        public Utility GetById(int id)
        {
            return _context.Utilities.FirstOrDefault(u => u.Id == id);
        }

        public void Add(Utility entity)
        {
            _context.Utilities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Utility entity)
        {
            _context.Utilities.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Utilities.FirstOrDefault(u => u.Id == id);
            if (entity != null)
            {
                _context.Utilities.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}