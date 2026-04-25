using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class UnifiedCommunicationsRepository : IUnifiedCommunicationsRepository
    {
        private readonly AppDbContext _context;

        public UnifiedCommunicationsRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UnifiedCommunications> GetAll()
        {
            return _context.UnifiedCommunications.ToList();
        }

        public UnifiedCommunications GetById(int id)
        {
            return _context.UnifiedCommunications.FirstOrDefault(u => u.Id == id);
        }

        public void Add(UnifiedCommunications entity)
        {
            _context.UnifiedCommunications.Add(entity);
            _context.SaveChanges();
        }

        public void Update(UnifiedCommunications entity)
        {
            _context.UnifiedCommunications.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.UnifiedCommunications.FirstOrDefault(u => u.Id == id);
            if (entity != null)
            {
                _context.UnifiedCommunications.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}