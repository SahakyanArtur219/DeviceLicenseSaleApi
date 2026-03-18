using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeviceLicenseSaleApi.Configuration;
using DeviceLicenseSaleApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DeviceLicenseSaleApi.Helpers
{
    public class JwtHelper
    {
        private readonly JwtOptions _options;

        public JwtHelper(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public DateTime GetExpirationUtc()
        {
            return DateTime.UtcNow.AddDays(_options.ExpireDays);
        }

        public string GenerateToken(User user)
        {
            var expiresAtUtc = GetExpirationUtc();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role),
                new("company_id", user.CompanyId.ToString()),
                new("building_id", user.BuildingId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
