using System.Text.Json;
using DeviceLicenseSaleApi.Configuration;
using DeviceLicenseSaleApi.Logging;
using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeviceLicenseSaleApi.Infrastructure.Logging
{
    public class ActivityLogger : IActivityLogger
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = false
        };

        private readonly ILogger<ActivityLogger> _logger;
        private readonly SemaphoreSlim _writeLock = new(1, 1);
        private readonly string _logDirectory;

        public ActivityLogger(
            ILogger<ActivityLogger> logger,
            IHostEnvironment hostEnvironment,
            IOptions<ActivityLoggingOptions> options)
        {
            _logger = logger;

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
                    timestampUtc = DateTime.UtcNow,
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
                    timestampUtc = DateTime.UtcNow,
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

            await _writeLock.WaitAsync(cancellationToken);
            try
            {
                await File.AppendAllTextAsync(filePath, serialized + Environment.NewLine, cancellationToken);
            }
            finally
            {
                _writeLock.Release();
            }
        }
    }
}
