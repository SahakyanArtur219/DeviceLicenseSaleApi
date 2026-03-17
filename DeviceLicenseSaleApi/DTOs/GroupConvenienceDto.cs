namespace DeviceLicenseSaleApi.DTOs
{
    public class GroupConvenienceDto
    {
        public int Id { get; set; }
        public string AutoRedial { get; set; }
        public string CallPaging { get; set; }
        public string SpeedDial { get; set; }
        public string PhoneBook { get; set; }
        public string Intercom { get; set; }
        // Expose only fields relevant for clients
    }
}