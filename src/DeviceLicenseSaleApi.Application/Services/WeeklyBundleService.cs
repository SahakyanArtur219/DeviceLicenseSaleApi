using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services;

public class WeeklyBundleService : IWeeklyBundleService
{
    private readonly IWeeklyBundleRepository _repository;

    public WeeklyBundleService(IWeeklyBundleRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<WeeklyBundleResponseDto> GetAll()
    {
        return _repository.GetAll().Select(Map);
    }

    public IEnumerable<WeeklyBundleResponseDto> GetActiveForDeviceType(int deviceTypeId)
    {
        return _repository.GetActiveForDeviceType(deviceTypeId, DateTime.UtcNow).Select(Map);
    }

    public WeeklyBundleResponseDto Create(WeeklyBundleCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Bundle name is required.");
        }

        if (dto.DiscountPercentage <= 0 || dto.DiscountPercentage >= 100)
        {
            throw new ArgumentException("Discount percentage must be between 0 and 100.");
        }

        if (dto.EndsAtUtc <= dto.StartsAtUtc)
        {
            throw new ArgumentException("Bundle end date must be after the start date.");
        }

        var featureKeys = dto.FeatureKeys
            .Where(key => !string.IsNullOrWhiteSpace(key))
            .Select(key => key.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (featureKeys.Count < 2)
        {
            throw new ArgumentException("A bundle must contain at least two features.");
        }

        var bundle = new WeeklyBundle
        {
            DeviceTypeId = dto.DeviceTypeId,
            Name = dto.Name.Trim(),
            DiscountPercentage = dto.DiscountPercentage,
            StartsAtUtc = dto.StartsAtUtc,
            EndsAtUtc = dto.EndsAtUtc,
            IsActive = true,
            Features = featureKeys.Select(key => new WeeklyBundleFeature { FeatureKey = key }).ToList()
        };

        return Map(_repository.Add(bundle));
    }

    public void Delete(int id)
    {
        var bundle = _repository.GetById(id);
        if (bundle == null)
        {
            return;
        }

        _repository.Delete(bundle);
    }

    private static WeeklyBundleResponseDto Map(WeeklyBundle bundle)
    {
        var now = DateTime.UtcNow;
        return new WeeklyBundleResponseDto
        {
            Id = bundle.Id,
            DeviceTypeId = bundle.DeviceTypeId,
            Name = bundle.Name,
            DiscountPercentage = bundle.DiscountPercentage,
            StartsAtUtc = bundle.StartsAtUtc,
            EndsAtUtc = bundle.EndsAtUtc,
            IsActive = bundle.IsActive && bundle.StartsAtUtc <= now && bundle.EndsAtUtc >= now,
            FeatureKeys = bundle.Features.Select(feature => feature.FeatureKey).ToList()
        };
    }
}