using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public UserResponseDto User { get; set; } = null!;
        public UserProfileResponseDto? Profile { get; set; }
    }
}
