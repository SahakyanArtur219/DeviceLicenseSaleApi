using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User? GetById(int id);
        User Add(User user);
        void Update(User user);
        void Delete(int id);
        User? GetByUsernameOrEmail(string value);
        bool ExistsByEmail(string email);
        bool ExistsByUsername(string username);
    }
}
