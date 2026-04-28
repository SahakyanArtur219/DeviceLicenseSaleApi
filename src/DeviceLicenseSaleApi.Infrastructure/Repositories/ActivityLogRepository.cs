using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly AppDbContext _context;

        public ActivityLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActivityLogSearchResultDto> SearchAsync(ActivityLogQueryDto query, CancellationToken cancellationToken = default)
        {
            var activityLogs = _context.ActivityLogs
                .AsNoTracking()
                .AsQueryable();

            if (query.FromUtc.HasValue)
            {
                activityLogs = activityLogs.Where(x => x.TimestampUtc >= query.FromUtc.Value);
            }

            if (query.ToUtc.HasValue)
            {
                activityLogs = activityLogs.Where(x => x.TimestampUtc <= query.ToUtc.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                var category = query.Category.Trim();
                activityLogs = activityLogs.Where(x => x.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(query.Action))
            {
                var action = query.Action.Trim();
                activityLogs = activityLogs.Where(x => x.Action.Contains(action));
            }

            if (!string.IsNullOrWhiteSpace(query.Outcome))
            {
                var outcome = query.Outcome.Trim();
                activityLogs = activityLogs.Where(x => x.Outcome == outcome);
            }

            if (query.UserId.HasValue)
            {
                activityLogs = activityLogs.Where(x => x.UserId == query.UserId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.Trim();
                activityLogs = activityLogs.Where(x =>
                    (x.Username != null && x.Username.Contains(searchTerm)) ||
                    (x.Description != null && x.Description.Contains(searchTerm)) ||
                    (x.EntityName != null && x.EntityName.Contains(searchTerm)) ||
                    (x.EntityId != null && x.EntityId.Contains(searchTerm)) ||
                    x.Action.Contains(searchTerm) ||
                    x.Category.Contains(searchTerm));
            }

            var totalCount = await activityLogs.CountAsync(cancellationToken);
            var succeededCount = await activityLogs.CountAsync(
                x => x.Outcome == "Succeeded",
                cancellationToken);
            var failedCount = await activityLogs.CountAsync(
                x => x.Outcome == "Failed",
                cancellationToken);

            var items = await activityLogs
                .OrderByDescending(x => x.TimestampUtc)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new ActivityLogItemDto
                {
                    Id = x.Id,
                    TimestampUtc = x.TimestampUtc,
                    Category = x.Category,
                    Action = x.Action,
                    Outcome = x.Outcome,
                    UserId = x.UserId,
                    Username = x.Username,
                    EntityName = x.EntityName,
                    EntityId = x.EntityId,
                    Description = x.Description,
                    TraceId = x.TraceId,
                    DetailsJson = x.DetailsJson
                })
                .ToListAsync(cancellationToken);

            return new ActivityLogSearchResultDto
            {
                TotalCount = totalCount,
                SucceededCount = succeededCount,
                FailedCount = failedCount,
                Items = items
            };
        }
    }
}
