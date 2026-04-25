namespace DeviceLicenseSaleApi.DTOs
{
    public class CallAnsweringDto
    {
        public int Id { get; set; }
        public string AutoAttendantStandardCustomScenarios { get; set; }
        public string CallHunting { get; set; }
        public string SimultaneousRing { get; set; }
        public string CallQueue { get; set; }
    }
}