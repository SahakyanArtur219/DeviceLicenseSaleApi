namespace DeviceLicenseSaleApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int BuildingId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Company Company { get; set; }

        public Building Building { get; set; }
    }
}