using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories;

public interface IWeeklyBundleRepository
{
    IEnumerable<WeeklyBundle> GetAll();
    IEnumerable<WeeklyBundle> GetActiveForDeviceType(int deviceTypeId, DateTime utcNow);
    WeeklyBundle Add(WeeklyBundle bundle);
    WeeklyBundle? GetById(int id);
    void Delete(WeeklyBundle bundle);
}