namespace DeviceLicenseSaleApi.DTOs.Auth
{
    public class LoginDto
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}