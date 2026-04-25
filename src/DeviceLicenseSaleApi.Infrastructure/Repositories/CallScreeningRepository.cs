using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class CallScreeningRepository : ICallScreeningRepository
    {
        private readonly AppDbContext _context;

        public CallScreeningRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CallScreening> GetAll()
        {
            return _context.CallScreenings.ToList();
        }

        public CallScreening GetById(int id)
        {
            return _context.CallScreenings.FirstOrDefault(c => c.Id == id);
        }

        public void Add(CallScreening callScreening)
        {
            _context.CallScreenings.Add(callScreening);
            _context.SaveChanges();
        }

        public void Update(CallScreening callScreening)
        {
            _context.CallScreenings.Update(callScreening);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.CallScreenings.FirstOrDefault(c => c.Id == id);
            if (entity != null)
            {
                _context.CallScreenings.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}