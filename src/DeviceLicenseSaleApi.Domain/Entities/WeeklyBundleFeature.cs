namespace DeviceLicenseSaleApi.Models;

public class WeeklyBundleFeature
{
    public int Id { get; set; }
    public int WeeklyBundleId { get; set; }
    public string FeatureKey { get; set; } = string.Empty;

    public WeeklyBundle? WeeklyBundle { get; set; }
}