using DeviceLicenseSaleApi.DTOs.Auth;

namespace DeviceLicenseSaleApi.Services.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDto Register(RegisterDto dto);
        AuthResponseDto Login(LoginDto dto);
    }
}