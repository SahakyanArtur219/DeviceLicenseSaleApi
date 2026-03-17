using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class CallAnsweringRepository : ICallAnsweringRepository
    {
        private readonly AppDbContext _context;

        public CallAnsweringRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CallAnswering> GetAll()
        {
            return _context.CallAnswerings.ToList();
        }

        public CallAnswering GetById(int id)
        {
            return _context.CallAnswerings.FirstOrDefault(c => c.Id == id);
        }

        public void Add(CallAnswering callAnswering)
        {
            _context.CallAnswerings.Add(callAnswering);
            _context.SaveChanges();
        }

        public void Update(CallAnswering callAnswering)
        {
            _context.CallAnswerings.Update(callAnswering);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.CallAnswerings.FirstOrDefault(c => c.Id == id);
            if (entity != null)
            {
                _context.CallAnswerings.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}