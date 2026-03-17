namespace DeviceLicenseSaleApi.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int BuildingId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}