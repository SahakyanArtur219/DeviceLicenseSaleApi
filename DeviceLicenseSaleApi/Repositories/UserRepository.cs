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

        public User GetById(int id)
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
        public User GetByUsernameOrEmail(string value)
        {
            return _context.Users
                .FirstOrDefault(x => x.Username == value || x.Email == value);
        }
        public bool ExistsByEmail(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        public bool ExistsByUsername(string username)
        {
            return _context.Users.Any(x => x.Username == username);
        }
    }
}