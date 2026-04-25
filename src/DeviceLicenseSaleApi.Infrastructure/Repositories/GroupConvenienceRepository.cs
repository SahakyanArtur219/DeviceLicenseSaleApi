using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public class GroupConvenienceRepository : IGroupConvenienceRepository
    {
        private readonly AppDbContext _context;

        public GroupConvenienceRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GroupConvenience> GetAll()
        {
            return _context.GroupConveniences.ToList();
        }

        public GroupConvenience GetById(int id)
        {
            return _context.GroupConveniences.FirstOrDefault(c => c.Id == id);
        }

        public void Add(GroupConvenience entity)
        {
            _context.GroupConveniences.Add(entity);
            _context.SaveChanges();
        }

        public void Update(GroupConvenience entity)
        {
            _context.GroupConveniences.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.GroupConveniences.FirstOrDefault(c => c.Id == id);
            if (entity != null)
            {
                _context.GroupConveniences.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}