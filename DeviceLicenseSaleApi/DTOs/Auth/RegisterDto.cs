namespace DeviceLicenseSaleApi.DTOs.Auth
{
    public class RegisterDto
    {
        public int CompanyId { get; set; }
        public int BuildingId { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}