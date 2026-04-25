namespace DeviceLicenseSaleApi.DTOs
{
    public class CallManagementDto
    {
        public int Id { get; set; }
        public string CallHold { get; set; }
        public string CallTransfer { get; set; }
        public string CallWaiting { get; set; }
        public string ThreeWayConferencing { get; set; }
    }
}