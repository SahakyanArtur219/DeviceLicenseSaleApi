using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AppDbContext _context;

        public DeviceRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Device> GetByUserId(int userId)
        {
            return _context.Devices
                .Include(x => x.DeviceType)
                .Include(x => x.License)
                .Where(x => x.UserId == userId)
                .ToList();
        }

        public Device GetByIdForUser(int id, int userId)
        {
            return _context.Devices
                .Include(x => x.DeviceType)
                .Include(x => x.License)
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);
        }

        public Device Add(Device device)
        {
            _context.Devices.Add(device);
            _context.SaveChanges();
            return device;
        }

        public void Update(Device device)
        {
            _context.Devices.Update(device);
            _context.SaveChanges();
        }

        public void Delete(Device device)
        {
            _context.Devices.Remove(device);
            _context.SaveChanges();
        }
    }
}