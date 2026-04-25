using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface ILicenseRepository
    {
        IEnumerable<License> GetAll();
        License GetById(int id);
        License Add(License license);
        void Update(License license);
        void Delete(int id);

        bool ExistsByLicenseKey(string licenseKey);
    }
}