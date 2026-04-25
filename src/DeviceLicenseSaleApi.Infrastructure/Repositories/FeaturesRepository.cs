using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories
{
    public class FeaturesRepository : IFeaturesRepository
    {
        private readonly AppDbContext _context;

        public FeaturesRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Features> GetAll()
        {
            return _context.Features
                .Include(f => f.Administrative)
                .Include(f => f.CallAnswering)
                .Include(f => f.CallManagement)
                .Include(f => f.CallScreening)
                .Include(f => f.GroupConvenience)
                .Include(f => f.CostBandwidthSaving)
                .Include(f => f.Utility)
                .Include(f => f.SecurityTools)
                .Include(f => f.UnifiedCommunications)
                .ToList();
        }

        public Features GetById(int id)
        {
            return _context.Features
                .Include(f => f.Administrative)
                .Include(f => f.CallAnswering)
                .Include(f => f.CallManagement)
                .Include(f => f.CallScreening)
                .Include(f => f.GroupConvenience)
                .Include(f => f.CostBandwidthSaving)
                .Include(f => f.Utility)
                .Include(f => f.SecurityTools)
                .Include(f => f.UnifiedCommunications)
                .FirstOrDefault(f => f.Id == id);
        }

        public void Add(Features entity)
        {
            _context.Features.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Features entity)
        {
            _context.Features.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Features.FirstOrDefault(f => f.Id == id);
            if (entity != null)
            {
                _context.Features.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}