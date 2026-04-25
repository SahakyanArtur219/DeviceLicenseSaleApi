namespace DeviceLicenseSaleApi.DTOs
{
    public class DeviceUpdateDto
    {
        public int DeviceTypeId { get; set; }
        public int? LicenseId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}