using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services
{
    public interface IUserService
    {
        IEnumerable<UserResponseDto> GetAll();
        UserResponseDto? GetById(int id);
        UserResponseDto Create(UserCreateDto dto);
        void Update(int id, UserUpdateDto dto);
        void Delete(int id);
    }
}
