namespace DeviceLicenseSaleApi.DTOs
{
    public class PurchaseLineItemDto
    {
        public string FeatureKey { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
    }

    public class AppliedBundleDto
    {
        public int BundleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<string> FeatureKeys { get; set; } = new();
    }

    public class PurchaseQuoteDto
    {
        public decimal Subtotal { get; set; }
        public decimal BundleDiscountAmount { get; set; }
        public decimal PointsDiscountAmount { get; set; }
        public decimal Total { get; set; }
        public int AvailableRewardPoints { get; set; }
        public int PointsRedeemed { get; set; }
        public int PointsEarned { get; set; }
        public List<AppliedBundleDto> AppliedBundles { get; set; } = new();
    }

    public class PayPalClientConfigDto
    {
        public string ClientId { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "USD";
    }

    public class PayPalCreateOrderRequestDto
    {
        public int DeviceId { get; set; }
        public int DeviceTypeId { get; set; }
        public int PointsToRedeem { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<PurchaseLineItemDto> LineItems { get; set; } = new();
    }

    public class PayPalCaptureOrderRequestDto
    {
        public int DeviceId { get; set; }
        public int DeviceTypeId { get; set; }
        public int PointsToRedeem { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string DeviceLocation { get; set; } = string.Empty;
        public List<PurchaseLineItemDto> LineItems { get; set; } = new();
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