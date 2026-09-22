using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services.Interfaces
{
    public interface IDeviceRecommendationService
    {
        DeviceRecommendationChatResponseDto Recommend(DeviceRecommendationChatRequestDto request);
    }
}
