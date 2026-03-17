namespace DeviceLicenseSaleApi.DTOs
{
    public class CreateCallManagementDto
    {
        public string CallHold { get; set; }
        public string CallPark { get; set; }
        public string CallParkOnAutoAttendant { get; set; }
        public string CallTransfer { get; set; }
        public string CallWaiting { get; set; }
        public string ThreeWayConferencing { get; set; }
        public string EmergencyInterrupt { get; set; }
        public string Scheduling { get; set; }
    }
}