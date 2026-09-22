using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services.Interfaces;

namespace DeviceLicenseSaleApi.Services
{
    public class UserService : IUserService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(
            IUserRepository repository,
            ICompanyRepository companyRepository,
            IBuildingRepository buildingRepository,
            IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _companyRepository = companyRepository;
            _buildingRepository = buildingRepository;
            _passwordHasher = passwordHasher;
        }

        public IEnumerable<UserResponseDto> GetAll()
        {
            return _repository.GetAll().Select(MapUser);
        }

        public UserResponseDto? GetById(int id)
        {
            var user = _repository.GetById(id);
            return user == null ? null : MapUser(user);
        }

        public UserResponseDto Create(UserCreateDto dto)
        {
            ValidateCompanyAndBuilding(dto.CompanyId, dto.BuildingId);

            var normalizedEmail = dto.Email.Trim();
            var normalizedUsername = dto.Username.Trim();

            if (_repository.ExistsByEmail(normalizedEmail))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            if (_repository.ExistsByUsername(normalizedUsername))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            var user = new User
            {
                CompanyId = dto.CompanyId,
                BuildingId = dto.BuildingId,
                Username = normalizedUsername,
                Email = normalizedEmail,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                Role = NormalizeRole(dto.Role),
                IsActive = true,
                RewardPoints = 0,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = _repository.Add(user);
            return MapUser(createdUser);
        }

        public void Update(int id, UserUpdateDto dto)
        {
            var user = _repository.GetById(id);

            if (user == null)
            {
                return;
            }

            ValidateCompanyAndBuilding(dto.CompanyId, dto.BuildingId);

            var normalizedEmail = dto.Email.Trim();
            var normalizedUsername = dto.Username.Trim();

            if (!string.Equals(user.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase) &&
                _repository.ExistsByEmail(normalizedEmail))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            if (!string.Equals(user.Username, normalizedUsername, StringComparison.OrdinalIgnoreCase) &&
                _repository.ExistsByUsername(normalizedUsername))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            user.CompanyId = dto.CompanyId;
            user.BuildingId = dto.BuildingId;
            user.Username = normalizedUsername;
            user.Email = normalizedEmail;
            user.Role = NormalizeRole(dto.Role);
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            _repository.Update(user);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private void ValidateCompanyAndBuilding(int companyId, int buildingId)
        {
            if (!_companyRepository.Exists(companyId))
            {
                throw new ArgumentException("Selected company does not exist.");
            }

            if (!_buildingRepository.Exists(buildingId))
            {
                throw new ArgumentException("Selected building does not exist.");
            }
        }

        private static string NormalizeRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return "User";
            }

            var normalized = role.Trim();

            if (normalized.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return "Admin";
            }

            if (normalized.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                return "User";
            }

            throw new ArgumentException("Role must be either 'Admin' or 'User'.");
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