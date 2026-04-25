namespace DeviceLicenseSaleApi.DTOs
{
    public class PayPalClientConfigDto
    {
        public string ClientId { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "USD";
    }

    public class PayPalCreateOrderRequestDto
    {
        public int DeviceId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class PayPalCaptureOrderRequestDto
    {
        public int DeviceId { get; set; }
        public int DeviceTypeId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string DeviceLocation { get; set; } = string.Empty;
        public LicenseCreateDto LicensePayload { get; set; } = new();
    }

    public class PayPalOrderResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class PayPalCaptureResponseDto
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? CaptureId { get; set; }
        public string? PayerEmail { get; set; }
    }
}
