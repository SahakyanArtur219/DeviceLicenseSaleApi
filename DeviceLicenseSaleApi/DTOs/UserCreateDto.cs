namespace DeviceLicenseSaleApi.DTOs
{
    public class UserCreateDto
    {
        public int CompanyId { get; set; }

        public int BuildingId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}