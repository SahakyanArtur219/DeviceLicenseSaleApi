using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services
{
    public interface IPayPalService
    {
        bool IsConfigured { get; }
        PayPalClientConfigDto GetClientConfig();
        Task<PayPalOrderResponseDto> CreateOrderAsync(PayPalCreateOrderRequestDto dto, CancellationToken cancellationToken = default);
        Task<PayPalCaptureResponseDto> CaptureOrderAsync(string orderId, CancellationToken cancellationToken = default);
    }
}
