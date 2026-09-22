using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.DTOs.Auth;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services.Interfaces;

namespace DeviceLicenseSaleApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityLogger _activityLogger;

        public AuthService(
            ICompanyRepository companyRepository,
            IBuildingRepository buildingRepository,
            IUserRepository userRepository,
            IUserProfileRepository profileRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
            IUnitOfWork unitOfWork,
            IActivityLogger activityLogger)
        {
            _companyRepository = companyRepository;
            _buildingRepository = buildingRepository;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
            _activityLogger = activityLogger;
        }

        public AuthResponseDto Register(RegisterDto dto)
        {
            if (!_companyRepository.Exists(dto.CompanyId))
            {
                throw new ArgumentException("Selected company does not exist.");
            }

            if (!_buildingRepository.Exists(dto.BuildingId))
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

            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var user = new User
                {
                    CompanyId = dto.CompanyId,
                    BuildingId = dto.BuildingId,
                    Username = normalizedUsername,
                    Email = normalizedEmail,
                    PasswordHash = _passwordHasher.Hash(dto.Password),
                    Role = "User",
                    IsActive = true,
                    RewardPoints = 0,
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
                    CreatedAt = DateTime.UtcNow
                };

                var createdProfile = _profileRepository.Add(userProfile);
                var expiresAtUtc = _jwtTokenService.GetExpirationUtc();
                var token = _jwtTokenService.GenerateToken(createdUser);

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

            if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            {
                _activityLogger.LogActivityAsync(new ActivityLogEntry
                {
                    Category = "Security",
                    Action = "Login",
                    Outcome = "Failed",
                    Description = "Invalid login credentials.",
                    Details = new Dictionary<string, object?>
                    {
                        ["usernameOrEmail"] = dto.UsernameOrEmail.Trim()
                    }
                }).GetAwaiter().GetResult();
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            if (!user.IsActive)
            {
                _activityLogger.LogActivityAsync(new ActivityLogEntry
                {
                    Category = "Security",
                    Action = "Login",
                    Outcome = "Failed",
                    UserId = user.Id,
                    Username = user.Username,
                    EntityName = "User",
                    EntityId = user.Id.ToString(),
                    Description = "Inactive user attempted to log in."
                }).GetAwaiter().GetResult();
                throw new InvalidOperationException("User account is inactive.");
            }

            var profile = _profileRepository.GetByUserId(user.Id);
            var expiresAtUtc = _jwtTokenService.GetExpirationUtc();
            var token = _jwtTokenService.GenerateToken(user);

            _activityLogger.LogActivityAsync(new ActivityLogEntry
            {
                Category = "Security",
                Action = "Login",
                Outcome = "Succeeded",
                UserId = user.Id,
                Username = user.Username,
                EntityName = "User",
                EntityId = user.Id.ToString(),
                Description = "User logged in successfully."
            }).GetAwaiter().GetResult();

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
                RewardPoints = user.RewardPoints,
                CreatedAt = user.CreatedAt
            };
        }
    }
}