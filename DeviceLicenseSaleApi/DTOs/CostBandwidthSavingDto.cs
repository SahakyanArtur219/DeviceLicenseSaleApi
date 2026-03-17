namespace DeviceLicenseSaleApi.DTOs
{
    public class CostBandwidthSavingDto
    {
        public int Id { get; set; }
        public string CallAlert { get; set; }
        public string DialPlans { get; set; }
        public string ClassOfService { get; set; }
        public string TimeOfDayDialing { get; set; }
        // Expose only fields relevant for clients
    }
}