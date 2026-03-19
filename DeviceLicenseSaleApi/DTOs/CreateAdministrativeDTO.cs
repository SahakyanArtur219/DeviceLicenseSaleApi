namespace DeviceLicenseSaleApi.DTOs
{
    public class CreateAdministrativeDto
    {
        public string AutomaticBackupDownloadQXConfiguration { get; set; }
        public string AutomaticFirmwareUpdatesForInstalledIPPhones { get; set; }
        public string AutomaticQXFirmwareUpdate { get; set; }
        public string DaylightSavingsTimeAdjustment { get; set; }
    }
}