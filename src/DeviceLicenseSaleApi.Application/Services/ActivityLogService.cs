using DeviceLicenseSaleApi.DTOs;
using DeviceLicenseSaleApi.Repositories;
using DeviceLicenseSaleApi.Services.Interfaces;

namespace DeviceLicenseSaleApi.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _repository;

        public ActivityLogService(IActivityLogRepository repository)
        {
            _repository = repository;
        }

        public Task<ActivityLogSearchResultDto> SearchAsync(ActivityLogQueryDto query, CancellationToken cancellationToken = default)
        {
            query.Page = query.Page < 1 ? 1 : query.Page;
            query.PageSize = query.PageSize switch
            {
                < 1 => 50,
                > 200 => 200,
                _ => query.PageSize
            };

            return _repository.SearchAsync(query, cancellationToken);
        }
    }
}
