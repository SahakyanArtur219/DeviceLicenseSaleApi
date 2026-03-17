using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class CallManagementRepository : ICallManagementRepository
    {
        private readonly AppDbContext _context;

        public CallManagementRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CallManagement> GetAll()
        {
            return _context.CallManagements.ToList();
        }

        public CallManagement GetById(int id)
        {
            return _context.CallManagements.FirstOrDefault(c => c.Id == id);
        }

        public void Add(CallManagement callManagement)
        {
            _context.CallManagements.Add(callManagement);
            _context.SaveChanges();
        }

        public void Update(CallManagement callManagement)
        {
            _context.CallManagements.Update(callManagement);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.CallManagements.FirstOrDefault(c => c.Id == id);
            if (entity != null)
            {
                _context.CallManagements.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}