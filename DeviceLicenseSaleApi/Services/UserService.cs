using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace DeviceLicenseSaleApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserResponseDto> GetAll()
        {
            return _repository.GetAll().Select(u => new UserResponseDto
            {
                Id = u.Id,
                CompanyId = u.CompanyId,
                BuildingId = u.BuildingId,
                Username = u.Username,
                Email = u.Email,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });
        }

        public UserResponseDto GetById(int id)
        {
            var u = _repository.GetById(id);

            if (u == null) return null;

            return new UserResponseDto
            {
                Id = u.Id,
                CompanyId = u.CompanyId,
                BuildingId = u.BuildingId,
                Username = u.Username,
                Email = u.Email,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            };
        }

        public UserResponseDto Create(UserCreateDto dto)
        {
            var user = new User
            {
                CompanyId = dto.CompanyId,
                BuildingId = dto.BuildingId,
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = _repository.Add(user);

            return new UserResponseDto
            {
                Id = createdUser.Id,
                CompanyId = createdUser.CompanyId,
                BuildingId = createdUser.BuildingId,
                Username = createdUser.Username,
                Email = createdUser.Email,
                IsActive = createdUser.IsActive,
                CreatedAt = createdUser.CreatedAt
            };
        }

        public void Update(int id, UserUpdateDto dto)
        {
            var user = _repository.GetById(id);

            if (user == null) return;

            user.CompanyId = dto.CompanyId;
            user.BuildingId = dto.BuildingId;
            user.Email = dto.Email;
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            _repository.Update(user);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}