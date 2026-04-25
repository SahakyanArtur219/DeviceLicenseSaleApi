using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly AppDbContext _context;

        public LicenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<License> GetAll()
        {
            return _context.Licenses
                .Include(x => x.Devices)
                .ToList();
        }

        public License GetById(int id)
        {
            return _context.Licenses
                .Include(x => x.Devices)
                .FirstOrDefault(x => x.Id == id);
        }

        public License Add(License license)
        {
            _context.Licenses.Add(license);
            _context.SaveChanges();
            return license;
        }

        public void Update(License license)
        {
            _context.Licenses.Update(license);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Licenses.Find(id);
            if (entity != null)
            {
                _context.Licenses.Remove(entity);
                _context.SaveChanges();
            }
        }

        public bool ExistsByLicenseKey(string licenseKey)
        {
            return _context.Licenses.Any(x => x.LicenseKey == licenseKey);
        }
    }
}