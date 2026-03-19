namespace DeviceLicenseSaleApi.DTOs
{
    public class AdministrativeDto
    {
        public int Id { get; set; }
        public string AutomaticBackupDownloadQXConfiguration { get; set; }
        public string AutomaticFirmwareUpdatesForInstalledIPPhones { get; set; }
        public string AutomaticQXFirmwareUpdate { get; set; }
        public string DaylightSavingsTimeAdjustment { get; set; }
    }
}