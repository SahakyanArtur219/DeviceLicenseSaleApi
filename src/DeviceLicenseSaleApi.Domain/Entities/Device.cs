namespace DeviceLicenseSaleApi.Models
{
    public class Device
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int DeviceTypeId { get; set; }
        public int? LicenseId { get; set; }

        public string Name { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Location { get; set; }

        public User User { get; set; }
        public DeviceTypes DeviceType { get; set; }
        public License? License { get; set; }
    }
}