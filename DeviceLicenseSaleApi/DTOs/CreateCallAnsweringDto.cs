namespace DeviceLicenseSaleApi.DTOs
{
    public class CreateCallAnsweringDto
    {
        public string AutoAttendantStandardCustomScenarios { get; set; }
        public string CallHistoryAutoAttendantCustomScenario { get; set; }
        public string CallHunting { get; set; }
        public string SimultaneousRing { get; set; }
        public string CallPickup { get; set; }
        public string CallQueue { get; set; }
        public string ExtensionStatus { get; set; }
        public string HotDesking { get; set; }
        public string MultiCompanyReceptionist { get; set; }
        public string EmergencyInterrupt { get; set; }
    }
}