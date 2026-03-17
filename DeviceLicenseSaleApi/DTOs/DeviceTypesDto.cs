namespace DeviceLicenseSaleApi.DTOs
{
    public class DeviceTypesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AnalogPhones { get; set; }
        public int IPPhones { get; set; }
        public int TotalPhones { get; set; }
        public int ConcurrentCalls { get; set; }
        public int EthernetLANPorts { get; set; }
        public int EthernetWANPorts { get; set; }
    }
}