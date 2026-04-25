namespace DeviceLicenseSaleApi.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TIN { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string? Website { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}