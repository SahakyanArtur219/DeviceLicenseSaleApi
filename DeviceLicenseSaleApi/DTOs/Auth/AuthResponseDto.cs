namespace DeviceLicenseSaleApi.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public UserResponseDto User { get; set; }

        public UserProfileResponseDto Profile { get; set; }
    }
}