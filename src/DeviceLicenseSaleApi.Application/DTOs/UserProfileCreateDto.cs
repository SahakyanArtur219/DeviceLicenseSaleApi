namespace DeviceLicenseSaleApi.DTOs
{
    public class UserProfileCreateDto
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
