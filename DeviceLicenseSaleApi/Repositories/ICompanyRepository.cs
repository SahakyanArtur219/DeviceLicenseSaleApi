using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAll();

        Company GetById(int id);

        void Add(Company company);

        void Update(Company company);

        void Delete(int id);
    }
}