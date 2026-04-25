using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class AdministrativeRepository : IAdministrativeRepository
    {
        private readonly AppDbContext _context;

        public AdministrativeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Administrative> GetAll()
        {
            return _context.Administratives.ToList();
        }

        public Administrative GetById(int id)
        {
            return _context.Administratives.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Administrative admin)
        {
            _context.Administratives.Add(admin);
            _context.SaveChanges();
        }

        public void Update(Administrative admin)
        {
            _context.Administratives.Update(admin);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var admin = _context.Administratives.FirstOrDefault(a => a.Id == id);
            if (admin != null)
            {
                _context.Administratives.Remove(admin);
                _context.SaveChanges();
            }
        }
    }
}