using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IUserProfileRepository
    {
        IEnumerable<UserProfile> GetAll();

        UserProfile GetById(int id);

        UserProfile GetByUserId(int userId);

        UserProfile Add(UserProfile profile);

        void Update(UserProfile profile);

        void Delete(int id);
    }
}