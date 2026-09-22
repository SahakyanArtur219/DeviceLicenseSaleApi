using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services;

public interface IPurchasePricingService
{
    PurchaseQuoteDto BuildQuote(int userId, int deviceTypeId, IEnumerable<PurchaseLineItemDto> lineItems, int pointsToRedeem);
    int FinalizeRewardPoints(int userId, PurchaseQuoteDto quote);
}