namespace DeviceLicenseSaleApi.DTOs
{
    public class DeviceResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int DeviceTypeId { get; set; }
        public int? LicenseId { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }
    }
}