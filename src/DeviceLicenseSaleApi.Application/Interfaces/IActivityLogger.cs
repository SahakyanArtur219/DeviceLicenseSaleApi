using DeviceLicenseSaleApi.Logging;

namespace DeviceLicenseSaleApi.Services.Interfaces
{
    public interface IActivityLogger
    {
        Task LogRequestAsync(RequestLogEntry entry, CancellationToken cancellationToken = default);
        Task LogActivityAsync(ActivityLogEntry entry, CancellationToken cancellationToken = default);
    }
}
