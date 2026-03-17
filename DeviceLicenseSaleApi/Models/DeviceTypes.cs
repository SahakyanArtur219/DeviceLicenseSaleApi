using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceLicenseSaleApi.Models
{
    public class DeviceTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AnalogPhones { get; set; }
        public int IPPhones { get; set; }
        public int AdditionalIPPhonesWithKeys { get; set; }
        public int TotalPhones { get; set; }
        public int ConcurrentCalls { get; set; }
        public int AdditionalConcurrentCallsWithKeys { get; set; }
        public int EthernetLANPorts { get; set; }
        public int EthernetWANPorts { get; set; }
        public int AudioInPorts { get; set; }
        public int AudioOutPorts { get; set; }
        public int SDSlots { get; set; }

        // Foreign Keys
        public int FeatureId { get; set; }
        public int LicensableFeatureId { get; set; }

        // Navigation Properties
        [ForeignKey("FeatureId")]
        public Features Features { get; set; }

        [ForeignKey("LicensableFeatureId")]
        public LicensableFeatures LicensableFeatures { get; set; }
    }
}