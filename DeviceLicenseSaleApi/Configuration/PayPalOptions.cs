namespace DeviceLicenseSaleApi.Configuration
{
    public class PayPalOptions
    {
        public const string SectionName = "PayPal";

        public string BaseUrl { get; set; } = "https://api-m.sandbox.paypal.com";
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "USD";
        public string MerchantEmail { get; set; } = string.Empty;
        public string BrandName { get; set; } = "Device License Sale";
    }
}
