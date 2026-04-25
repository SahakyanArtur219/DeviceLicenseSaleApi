using System.Collections.Generic;
using System.Linq;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories
{
    public class DeviceTypesRepository : IDeviceTypesRepository
    {
        private readonly AppDbContext _context;

        public DeviceTypesRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DeviceTypes> GetAll()
        {
            return _context.DeviceTypes
                .Include(d => d.Features)
                .Include(d => d.LicensableFeatures)
                .ToList();
        }

        public DeviceTypes GetById(int id)
        {
            return _context.DeviceTypes
                .Include(d => d.Features)
                .Include(d => d.LicensableFeatures)
                .FirstOrDefault(d => d.Id == id);
        }

        public void Add(DeviceTypes entity)
        {
            _context.DeviceTypes.Add(entity);
            _context.SaveChanges();
        }

        public void Update(DeviceTypes entity)
        {
            _context.DeviceTypes.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.DeviceTypes.FirstOrDefault(d => d.Id == id);
            if (entity != null)
            {
                _context.DeviceTypes.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}