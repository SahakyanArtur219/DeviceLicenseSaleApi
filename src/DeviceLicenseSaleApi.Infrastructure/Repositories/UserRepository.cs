using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .Include(u => u.Company)
                .Include(u => u.Building)
                .ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users
                .Include(u => u.Company)
                .Include(u => u.Building)
                .FirstOrDefault(u => u.Id == id);
        }

        public User Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User? GetByUsernameOrEmail(string value)
        {
            var normalized = Normalize(value);

            return _context.Users.FirstOrDefault(x =>
                x.Username.ToUpper() == normalized ||
                x.Email.ToUpper() == normalized);
        }

        public bool ExistsByEmail(string email)
        {
            var normalized = Normalize(email);
            return _context.Users.Any(x => x.Email.ToUpper() == normalized);
        }

        public bool ExistsByUsername(string username)
        {
            var normalized = Normalize(username);
            return _context.Users.Any(x => x.Username.ToUpper() == normalized);
        }

        private static string Normalize(string value)
        {
            return value.Trim().ToUpperInvariant();
        }
    }
}
