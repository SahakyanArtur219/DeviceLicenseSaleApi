namespace DeviceLicenseSaleApi.Configuration
{
    public class ActivityLoggingOptions
    {
        public const string SectionName = "ActivityLogging";

        public string Directory { get; set; } = "logs";
    }
}
