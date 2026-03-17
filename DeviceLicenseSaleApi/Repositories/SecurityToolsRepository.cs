using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class SecurityToolsRepository : ISecurityToolsRepository
    {
        private readonly AppDbContext _context;

        public SecurityToolsRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SecurityTools> GetAll()
        {
            return _context.SecurityTools.ToList();
        }

        public SecurityTools GetById(int id)
        {
            return _context.SecurityTools.FirstOrDefault(s => s.Id == id);
        }

        public void Add(SecurityTools entity)
        {
            _context.SecurityTools.Add(entity);
            _context.SaveChanges();
        }

        public void Update(SecurityTools entity)
        {
            _context.SecurityTools.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.SecurityTools.FirstOrDefault(s => s.Id == id);
            if (entity != null)
            {
                _context.SecurityTools.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}