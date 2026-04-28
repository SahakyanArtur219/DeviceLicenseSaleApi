using DeviceLicenseSaleApi.DTOs;

namespace DeviceLicenseSaleApi.Repositories
{
    public interface IActivityLogRepository
    {
        Task<ActivityLogSearchResultDto> SearchAsync(ActivityLogQueryDto query, CancellationToken cancellationToken = default);
    }
}
