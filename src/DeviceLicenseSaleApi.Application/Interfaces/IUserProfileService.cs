using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services
{
    public interface IUserProfileService
    {
        IEnumerable<UserProfileResponseDto> GetAll();

        UserProfileResponseDto GetById(int id);

        UserProfileResponseDto GetByUserId(int userId);

        void Create(UserProfileCreateDto dto);

        void Update(int id, UserProfileUpdateDto dto);

        void Delete(int id);
    }
}