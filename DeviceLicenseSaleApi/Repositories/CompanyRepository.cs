using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Data;

namespace DeviceLicenseSaleApi.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Company> GetAll()
        {
            return _context.Companies.ToList();
        }

        public Company GetById(int id)
        {
            return _context.Companies.Find(id);
        }

        public bool Exists(int id)
        {
            return _context.Companies.Any(x => x.Id == id);
        }

        public void Add(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
        }

        public void Update(Company company)
        {
            _context.Companies.Update(company);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var company = _context.Companies.Find(id);

            if (company != null)
            {
                _context.Companies.Remove(company);
                _context.SaveChanges();
            }
        }
    }
}
