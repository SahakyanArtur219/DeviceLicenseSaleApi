namespace DeviceLicenseSaleApi.DTOs
{
    public class DeviceRecommendationOptionDto
    {
        public int DeviceTypeId { get; set; }
        public string DeviceTypeName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int ProvidedIpPhones { get; set; }
        public int ProvidedAnalogPhones { get; set; }
        public int ProvidedConcurrentCalls { get; set; }
        public int ProvidedTotalPhones { get; set; }
        public int BaseIpPhones { get; set; }
        public int LicensableIpPhones { get; set; }
        public bool UsesLicensableIpPhones { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}