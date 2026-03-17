namespace DeviceLicenseSaleApi.DTOs
{
    public class UserUpdateDto
    {
        public int CompanyId { get; set; }

        public int BuildingId { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}