namespace DeviceLicenseSaleApi.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? CreatedAt { get; set; }

        public User User { get; set; }
    }
}
