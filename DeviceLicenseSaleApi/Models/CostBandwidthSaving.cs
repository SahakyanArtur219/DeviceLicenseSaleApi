namespace DeviceLicenseSaleApi.Models
{
    public class CostBandwidthSaving
    {
        public int Id { get; set; }
        public string CallAlert { get; set; }
        public string CallHistory { get; set; }
        public string DialPlans { get; set; }
        public string ClassOfService { get; set; }
        public string DateTimeSettings { get; set; }
        public string TimeOfDayDialing { get; set; }
        public string PinBarring { get; set; }
        public string OverallCallDurationLimit { get; set; }
        public string HotDesking { get; set; }
    }
}