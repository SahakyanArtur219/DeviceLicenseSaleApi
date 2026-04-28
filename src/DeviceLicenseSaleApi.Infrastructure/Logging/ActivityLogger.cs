using System.Text.Json;
using DeviceLicenseSaleApi.Configuration;
using DeviceLicenseSaleApi.Data;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Services.Interfaces;
using DeviceLicenseSaleApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceLicenseSaleApi.Infrastructure.Logging
{
    public class ActivityLogger : IActivityLogger
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = false
        };

        private readonly ILogger<ActivityLogger> _logger;
        private static readonly SemaphoreSlim WriteLock = new(1, 1);
        private readonly string _logDirectory;
        private readonly IServiceScopeFactory _scopeFactory;

        public ActivityLogger(
            ILogger<ActivityLogger> logger,
            IHostEnvironment hostEnvironment,
            IServiceScopeFactory scopeFactory,
            IOptions<ActivityLoggingOptions> options)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            var configuredDirectory = options.Value.Directory;
            _logDirectory = Path.IsPathRooted(configuredDirectory)
                ? configuredDirectory
                : Path.Combine(hostEnvironment.ContentRootPath, configuredDirectory);

            Directory.CreateDirectory(_logDirectory);
        }

        public async Task LogRequestAsync(RequestLogEntry entry, CancellationToken cancellationToken = default)
        {
            try
            {
                var timestampUtc = DateTime.UtcNow;

                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {DurationMs} ms. TraceId: {TraceId}, UserId: {UserId}",
                    entry.Method,
                    entry.Path,
                    entry.StatusCode,
                    entry.DurationMs,
                    entry.TraceId,
                    entry.UserId);

                var payload = new
                {
                    timestampUtc,
                    type = "request",
                    entry.Method,
                    entry.Path,
                    entry.QueryString,
                    entry.StatusCode,
                    entry.DurationMs,
                    entry.RemoteIpAddress,
                    entry.UserId,
                    entry.Username,
                    entry.TraceId
                };

                await WriteEntryAsync("requests", payload, cancellationToken);
                await SaveRequestLogAsync(timestampUtc, entry, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to write request log entry.");
            }
        }

        public async Task LogActivityAsync(ActivityLogEntry entry, CancellationToken cancellationToken = default)
        {
            try
            {
                var timestampUtc = DateTime.UtcNow;

                if (string.Equals(entry.Outcome, "Failed", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(
                        "{Category}::{Action} failed for UserId {UserId}. TraceId: {TraceId}. {Description}",
                        entry.Category,
                        entry.Action,
                        entry.UserId,
                        entry.TraceId,
                        entry.Description);
                }
                else
                {
                    _logger.LogInformation(
                        "{Category}::{Action} succeeded for UserId {UserId}. TraceId: {TraceId}. {Description}",
                        entry.Category,
                        entry.Action,
                        entry.UserId,
                        entry.TraceId,
                        entry.Description);
                }

                var payload = new
                {
                    timestampUtc,
                    type = "activity",
                    entry.Category,
                    entry.Action,
                    entry.Outcome,
                    entry.UserId,
                    entry.Username,
                    entry.EntityName,
                    entry.EntityId,
                    entry.Description,
                    entry.TraceId,
                    entry.Details
                };

                await WriteEntryAsync("activities", payload, cancellationToken);
                await SaveActivityLogAsync(timestampUtc, entry, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to write activity log entry.");
            }
        }

        private async Task WriteEntryAsync(string prefix, object payload, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_logDirectory, $"{prefix}-{DateTime.UtcNow:yyyyMMdd}.log");
            var serialized = JsonSerializer.Serialize(payload, SerializerOptions);

            await WriteLock.WaitAsync(cancellationToken);
            try
            {
                await File.AppendAllTextAsync(filePath, serialized + Environment.NewLine, cancellationToken);
            }
            finally
            {
                WriteLock.Release();
            }
        }

        private async Task SaveRequestLogAsync(DateTime timestampUtc, RequestLogEntry entry, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.RequestLogs.Add(new RequestLog
            {
                TimestampUtc = timestampUtc,
                Method = entry.Method,
                Path = entry.Path,
                QueryString = entry.QueryString,
                StatusCode = entry.StatusCode,
                DurationMs = entry.DurationMs,
                RemoteIpAddress = entry.RemoteIpAddress,
                UserId = entry.UserId,
                Username = entry.Username,
                TraceId = entry.TraceId
            });

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SaveActivityLogAsync(DateTime timestampUtc, ActivityLogEntry entry, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.ActivityLogs.Add(new ActivityLog
            {
                TimestampUtc = timestampUtc,
                Category = entry.Category,
                Action = entry.Action,
                Outcome = entry.Outcome,
                UserId = entry.UserId,
                Username = entry.Username,
                EntityName = entry.EntityName,
                EntityId = entry.EntityId,
                Description = entry.Description,
                TraceId = entry.TraceId,
                DetailsJson = entry.Details == null
                    ? null
                    : JsonSerializer.Serialize(entry.Details, SerializerOptions)
            });

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
