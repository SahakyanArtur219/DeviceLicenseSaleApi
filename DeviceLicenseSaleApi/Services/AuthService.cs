using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.DTOs.Auth;
using DeviceLicenseSaleApi.Helpers;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services.Interfaces;

namespace DeviceLicenseSaleApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(
            AppDbContext dbContext,
            IUserRepository userRepository,
            IUserProfileRepository profileRepository,
            JwtHelper jwtHelper)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _jwtHelper = jwtHelper;
        }

        public AuthResponseDto Register(RegisterDto dto)
        {
            if (!_dbContext.Companies.Any(x => x.Id == dto.CompanyId))
            {
                throw new ArgumentException("Selected company does not exist.");
            }

            if (!_dbContext.Buildings.Any(x => x.Id == dto.BuildingId))
            {
                throw new ArgumentException("Selected building does not exist.");
            }

            var normalizedEmail = dto.Email.Trim();
            var normalizedUsername = dto.Username.Trim();

            if (_userRepository.ExistsByEmail(normalizedEmail))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            if (_userRepository.ExistsByUsername(normalizedUsername))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var user = new User
                {
                    CompanyId = dto.CompanyId,
                    BuildingId = dto.BuildingId,
                    Username = normalizedUsername,
                    Email = normalizedEmail,
                    PasswordHash = PasswordHasher.Hash(dto.Password),
                    Role = "User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = _userRepository.Add(user);

                var userProfile = new UserProfile
                {
                    UserId = createdUser.Id,
                    FirstName = dto.FirstName.Trim(),
                    LastName = dto.LastName.Trim(),
                    Phone = dto.Phone?.Trim() ?? string.Empty,
                    Address = dto.Address?.Trim() ?? string.Empty,
                    DateOfBirth = dto.DateOfBirth,
                    CreatedAt = DateTime.UtcNow
                };

                var createdProfile = _profileRepository.Add(userProfile);
                var expiresAtUtc = _jwtHelper.GetExpirationUtc();
                var token = _jwtHelper.GenerateToken(createdUser);

                transaction.Commit();

                return new AuthResponseDto
                {
                    Token = token,
                    ExpiresAtUtc = expiresAtUtc,
                    User = MapUser(createdUser),
                    Profile = new UserProfileResponseDto
                    {
                        Id = createdProfile.Id,
                        UserId = createdProfile.UserId,
                        FirstName = createdProfile.FirstName,
                        LastName = createdProfile.LastName,
                        Phone = createdProfile.Phone,
                        Address = createdProfile.Address,
                        DateOfBirth = createdProfile.DateOfBirth,
                        CreatedAt = createdProfile.CreatedAt
                    }
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public AuthResponseDto Login(LoginDto dto)
        {
            var user = _userRepository.GetByUsernameOrEmail(dto.UsernameOrEmail.Trim());

            if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException("User account is inactive.");
            }

            var profile = _profileRepository.GetByUserId(user.Id);
            var expiresAtUtc = _jwtHelper.GetExpirationUtc();
            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                User = MapUser(user),
                Profile = profile == null
                    ? null
                    : new UserProfileResponseDto
                    {
                        Id = profile.Id,
                        UserId = profile.UserId,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Phone = profile.Phone,
                        Address = profile.Address,
                        DateOfBirth = profile.DateOfBirth,
                        CreatedAt = profile.CreatedAt
                    }
            };
        }

        private static UserResponseDto MapUser(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                CompanyId = user.CompanyId,
                BuildingId = user.BuildingId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
