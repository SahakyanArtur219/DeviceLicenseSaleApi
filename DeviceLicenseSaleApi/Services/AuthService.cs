using DeviceLicenseSaleApi.DTOs.Auth;
using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Helpers;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services.Interfaces;

namespace DeviceLicenseSaleApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, IUserProfileRepository profileRepository, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _jwtHelper = jwtHelper;
        }

        public AuthResponseDto Register(RegisterDto dto)
        {
            if (_userRepository.ExistsByEmail(dto.Email))
                throw new Exception("Email already exists");

            if (_userRepository.ExistsByUsername(dto.Username))
                throw new Exception("Username already exists");

            var user = new User
            {
                CompanyId = dto.CompanyId,
                BuildingId = dto.BuildingId,
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = _userRepository.Add(user);
            
            var userProfile = new UserProfile
            {
                UserId = createdUser.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Address = dto.Address,
                DateOfBirth = dto.DateOfBirth
            };

            var createdProfile = _profileRepository.Add(userProfile);

            var token = _jwtHelper.GenerateToken(createdUser);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserResponseDto
                {
                    Id = createdUser.Id,
                    CompanyId = createdUser.CompanyId,
                    BuildingId = createdUser.BuildingId,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    IsActive = createdUser.IsActive,
                    CreatedAt = createdUser.CreatedAt
                },
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

        public AuthResponseDto Login(LoginDto dto)
        {
            var user = _userRepository.GetByUsernameOrEmail(dto.UsernameOrEmail);

            if (user == null)
                throw new Exception("Invalid credentials");

            if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var profile = _profileRepository.GetByUserId(user.Id);

            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserResponseDto
                {
                    Id = user.Id,
                    CompanyId = user.CompanyId,
                    BuildingId = user.BuildingId,
                    Username = user.Username,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                },
                Profile = profile == null ? null : new UserProfileResponseDto
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
    }
}