using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceLicenseSaleApi.Models;

public class WeeklyBundle
{
    public int Id { get; set; }
    public int DeviceTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    [Column(TypeName = "decimal(5,2)")]
    public decimal DiscountPercentage { get; set; }
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DeviceTypes? DeviceType { get; set; }
    public ICollection<WeeklyBundleFeature> Features { get; set; } = new List<WeeklyBundleFeature>();
}