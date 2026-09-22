namespace DeviceLicenseSaleApi.DTOs
{
    public class DeviceRecommendationRequirementDto
    {
        public string Name { get; set; } = string.Empty;
        public int Requested { get; set; }
        public bool Inferred { get; set; }
        public string? SourceText { get; set; }
    }
}
