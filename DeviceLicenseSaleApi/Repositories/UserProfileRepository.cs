using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return _context.UserProfiles
                .Include(p => p.User)
                .ToList();
        }

        public UserProfile GetById(int id)
        {
            return _context.UserProfiles
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == id);
        }

        public UserProfile GetByUserId(int userId)
        {
            return _context.UserProfiles
                .Include(p => p.User)
                .FirstOrDefault(p => p.UserId == userId);
        }

        public UserProfile Add(UserProfile profile)
        {
            _context.UserProfiles.Add(profile);
            _context.SaveChanges();
            return profile;
        }

        public void Update(UserProfile profile)
        {
            _context.UserProfiles.Update(profile);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var profile = _context.UserProfiles.Find(id);
            if (profile != null)
            {
                _context.UserProfiles.Remove(profile);
                _context.SaveChanges();
            }
        }
    }
}