namespace DeviceLicenseSaleApi.DTOs
{
    public class DeviceRecommendationChatResponseDto
    {
        public string Summary { get; set; } = string.Empty;
        public string AssistantMessage { get; set; } = string.Empty;
        public IReadOnlyCollection<DeviceRecommendationRequirementDto> Requirements { get; set; } = Array.Empty<DeviceRecommendationRequirementDto>();
        public IReadOnlyCollection<DeviceRecommendationOptionDto> Recommendations { get; set; } = Array.Empty<DeviceRecommendationOptionDto>();
        public IReadOnlyCollection<string> Notes { get; set; } = Array.Empty<string>();
    }
}
