using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _repository;

        public UserProfileService(IUserProfileRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UserProfileResponseDto> GetAll()
        {
            return _repository.GetAll().Select(p => new UserProfileResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Phone = p.Phone,
                Address = p.Address,
                DateOfBirth = p.DateOfBirth,
                CreatedAt = p.CreatedAt
            });
        }

        public UserProfileResponseDto GetById(int id)
        {
            var p = _repository.GetById(id);
            if (p == null) return null;

            return new UserProfileResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Phone = p.Phone,
                Address = p.Address,
                DateOfBirth = p.DateOfBirth,
                CreatedAt = p.CreatedAt
            };
        }

        public UserProfileResponseDto GetByUserId(int userId)
        {
            var p = _repository.GetByUserId(userId);
            if (p == null) return null;

            return new UserProfileResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Phone = p.Phone,
                Address = p.Address,
                DateOfBirth = p.DateOfBirth,
                CreatedAt = p.CreatedAt
            };
        }

        public void Create(UserProfileCreateDto dto)
        {
            var profile = new UserProfile
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Address = dto.Address,
                DateOfBirth = dto.DateOfBirth,
                CreatedAt = DateTime.UtcNow
            };

            _repository.Add(profile);
        }

        public void Update(int id, UserProfileUpdateDto dto)
        {
            var profile = _repository.GetById(id);
            if (profile == null) return;

            profile.FirstName = dto.FirstName;
            profile.LastName = dto.LastName;
            profile.Phone = dto.Phone;
            profile.Address = dto.Address;
            profile.DateOfBirth = dto.DateOfBirth;

            _repository.Update(profile);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}