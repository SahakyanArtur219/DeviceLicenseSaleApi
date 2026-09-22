using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories;

public class WeeklyBundleRepository : IWeeklyBundleRepository
{
    private readonly AppDbContext _context;

    public WeeklyBundleRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<WeeklyBundle> GetAll()
    {
        return _context.Set<WeeklyBundle>()
            .Include(bundle => bundle.Features)
            .OrderByDescending(bundle => bundle.StartsAtUtc)
            .ToList();
    }

    public IEnumerable<WeeklyBundle> GetActiveForDeviceType(int deviceTypeId, DateTime utcNow)
    {
        return _context.Set<WeeklyBundle>()
            .Include(bundle => bundle.Features)
            .Where(bundle => bundle.DeviceTypeId == deviceTypeId && bundle.IsActive && bundle.StartsAtUtc <= utcNow && bundle.EndsAtUtc >= utcNow)
            .ToList();
    }

    public WeeklyBundle Add(WeeklyBundle bundle)
    {
        _context.Set<WeeklyBundle>().Add(bundle);
        _context.SaveChanges();
        return _context.Set<WeeklyBundle>()
            .Include(item => item.Features)
            .Single(item => item.Id == bundle.Id);
    }

    public WeeklyBundle? GetById(int id)
    {
        return _context.Set<WeeklyBundle>()
            .Include(bundle => bundle.Features)
            .SingleOrDefault(bundle => bundle.Id == id);
    }

    public void Delete(WeeklyBundle bundle)
    {
        _context.Set<WeeklyBundle>().Remove(bundle);
        _context.SaveChanges();
    }
}