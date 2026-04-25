namespace DeviceLicenseSaleApi.DTOs
{
    public class BuildingResponseDto
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}