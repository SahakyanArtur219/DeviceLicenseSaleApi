using System.Collections.Generic;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface ILicensableFeaturesRepository
    {
        IEnumerable<LicensableFeatures> GetAll();
        LicensableFeatures GetById(int id);
        void Add(LicensableFeatures entity);
        void Update(LicensableFeatures entity);
        void Delete(int id);
    }
}