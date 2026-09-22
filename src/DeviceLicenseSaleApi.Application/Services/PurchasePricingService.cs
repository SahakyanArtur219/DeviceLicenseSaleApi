using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Repositories;

namespace DeviceLicenseSaleApi.Services;

public class PurchasePricingService : IPurchasePricingService
{
    private static readonly Dictionary<string, decimal> FeaturePrices = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ipPhones"] = 25m,
        ["concurrentCalls"] = 40m,
        ["callRecording"] = 18m,
        ["callingCostControl"] = 120m,
        ["audioConferenceBridge"] = 35m,
        ["videoConferenceBridge"] = 55m,
        ["thirdPartyCallControl3PCC"] = 210m,
        ["advancedProxyConnectionService"] = 160m,
        ["eQallSoftphone"] = 30m,
        ["eQallReceptionistConsole"] = 70m,
        ["eQallSMSWhatsAppMessaging"] = 22m,
        ["CRMIntegration"] = 240m,
        ["voiceMailCallRecordingTranscription"] = 28m,
        ["textToSpeechTranscription"] = 140m,
        ["voiceEnabledAutoAttendant"] = 180m,
        ["automaticCallDistributionACD"] = 260m,
        ["epygiACDConsoleEAC"] = 85m,
        ["automaticOutboundCallingAOC"] = 225m,
        ["bargeIn"] = 95m,
        ["autoDialerActivation"] = 200m,
        ["autoDialerExpansionKey"] = 48m,
        ["PCCActivationLicense"] = 155m,
        ["serverSystemRedundancyActivation"] = 320m
    };

    private readonly IUserRepository _userRepository;
    private readonly IWeeklyBundleRepository _weeklyBundleRepository;

    public PurchasePricingService(IUserRepository userRepository, IWeeklyBundleRepository weeklyBundleRepository)
    {
        _userRepository = userRepository;
        _weeklyBundleRepository = weeklyBundleRepository;
    }

    public PurchaseQuoteDto BuildQuote(int userId, int deviceTypeId, IEnumerable<PurchaseLineItemDto> lineItems, int pointsToRedeem)
    {
        var user = _userRepository.GetById(userId) ?? throw new InvalidOperationException("User not found.");
        var normalizedItems = NormalizeItems(lineItems);
        var subtotal = normalizedItems.Sum(item => item.Quantity * GetPrice(item.FeatureKey));
        var activeBundles = _weeklyBundleRepository.GetActiveForDeviceType(deviceTypeId, DateTime.UtcNow).ToList();
        var appliedBundles = BuildAppliedBundles(normalizedItems, activeBundles);
        var bundleDiscount = appliedBundles.Sum(bundle => bundle.DiscountAmount);
        var availablePoints = Math.Max(0, user.RewardPoints);
        var maxRedeemablePoints = (int)Math.Floor(Math.Max(0m, subtotal - bundleDiscount));
        var redeemedPoints = Math.Max(0, Math.Min(pointsToRedeem, Math.Min(availablePoints, maxRedeemablePoints)));
        var total = Math.Max(0m, subtotal - bundleDiscount - redeemedPoints);
        var pointsEarned = (int)Math.Floor(total / 10m);

        return new PurchaseQuoteDto
        {
            Subtotal = subtotal,
            BundleDiscountAmount = bundleDiscount,
            PointsDiscountAmount = redeemedPoints,
            Total = total,
            AvailableRewardPoints = availablePoints,
            PointsRedeemed = redeemedPoints,
            PointsEarned = pointsEarned,
            AppliedBundles = appliedBundles
        };
    }

    public int FinalizeRewardPoints(int userId, PurchaseQuoteDto quote)
    {
        var user = _userRepository.GetById(userId) ?? throw new InvalidOperationException("User not found.");
        user.RewardPoints = Math.Max(0, user.RewardPoints - quote.PointsRedeemed + quote.PointsEarned);
        user.UpdatedAt = DateTime.UtcNow;
        _userRepository.Update(user);
        return user.RewardPoints;
    }

    private static List<PurchaseLineItemDto> NormalizeItems(IEnumerable<PurchaseLineItemDto> lineItems)
    {
        return lineItems
            .Where(item => !string.IsNullOrWhiteSpace(item.FeatureKey) && item.Quantity > 0)
            .GroupBy(item => item.FeatureKey.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(group => new PurchaseLineItemDto
            {
                FeatureKey = group.Key,
                Quantity = group.Sum(item => item.Quantity)
            })
            .ToList();
    }

    private static decimal GetPrice(string featureKey)
    {
        if (!FeaturePrices.TryGetValue(featureKey.Trim(), out var price))
        {
            throw new InvalidOperationException($"Unknown feature key: {featureKey}");
        }

        return price;
    }

    private static List<AppliedBundleDto> BuildAppliedBundles(
        IReadOnlyCollection<PurchaseLineItemDto> lineItems,
        IReadOnlyCollection<Models.WeeklyBundle> bundles)
    {
        var purchasedKeys = lineItems.Select(item => item.FeatureKey).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var assignedFeatures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var qualifiedBundles = bundles
            .Where(bundle => bundle.Features.Count > 0 && bundle.Features.All(feature => purchasedKeys.Contains(feature.FeatureKey)))
            .OrderByDescending(bundle => bundle.DiscountPercentage)
            .ThenBy(bundle => bundle.Id)
            .ToList();

        var applied = new List<AppliedBundleDto>();

        foreach (var bundle in qualifiedBundles)
        {
            var bundleKeys = bundle.Features.Select(feature => feature.FeatureKey).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var availableKeys = bundleKeys.Where(key => !assignedFeatures.Contains(key)).ToList();
            if (availableKeys.Count != bundleKeys.Count)
            {
                continue;
            }

            var bundleSubtotal = lineItems
                .Where(item => availableKeys.Contains(item.FeatureKey, StringComparer.OrdinalIgnoreCase))
                .Sum(item => item.Quantity * GetPrice(item.FeatureKey));

            if (bundleSubtotal <= 0)
            {
                continue;
            }

            foreach (var key in availableKeys)
            {
                assignedFeatures.Add(key);
            }

            applied.Add(new AppliedBundleDto
            {
                BundleId = bundle.Id,
                Name = bundle.Name,
                DiscountPercentage = bundle.DiscountPercentage,
                DiscountAmount = Math.Round(bundleSubtotal * (bundle.DiscountPercentage / 100m), 2),
                FeatureKeys = bundleKeys
            });
        }

        return applied;
    }
}