using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Services.Interfaces
{
    public interface IJwtTokenService
    {
        DateTime GetExpirationUtc();
        string GenerateToken(User user);
    }
}
