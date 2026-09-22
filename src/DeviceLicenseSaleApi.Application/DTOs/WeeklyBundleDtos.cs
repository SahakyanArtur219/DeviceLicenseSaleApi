namespace DeviceLicenseSaleApi.DTOs;

public class WeeklyBundleCreateDto
{
    public int DeviceTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public List<string> FeatureKeys { get; set; } = new();
}

public class WeeklyBundleResponseDto
{
    public int Id { get; set; }
    public int DeviceTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public bool IsActive { get; set; }
    public List<string> FeatureKeys { get; set; } = new();
}