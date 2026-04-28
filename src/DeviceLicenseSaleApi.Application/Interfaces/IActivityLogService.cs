using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task<ActivityLogSearchResultDto> SearchAsync(ActivityLogQueryDto query, CancellationToken cancellationToken = default);
    }
}
