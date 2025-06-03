using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType, DisableRequestTimeout]
    public abstract class BaseMcpTool
    {
        protected readonly ILogger _logger;
        protected readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private const int DEFAULT_TIMEOUT_SECONDS = 300;

        protected BaseMcpTool(UnitOfWorkInfo<MixCmsContext> cmsUow, ILogger logger)
        {
            _cmsUow = cmsUow;
            _logger = logger;
        }

        protected async Task<T> ExecuteWithExceptionHandlingAsync<T>(Func<CancellationToken, Task<T>> action, string operationName, int? timeoutSeconds = null)
        {
            using var cts = new CancellationTokenSource(timeoutSeconds.HasValue ? TimeSpan.FromSeconds(timeoutSeconds.Value) : TimeSpan.FromSeconds(DEFAULT_TIMEOUT_SECONDS));
            {
                try
                {
                    _logger.LogInformation("Starting {OperationName}", operationName);
                    var result = await action(cts.Token);
                    await _cmsUow.CompleteAsync(cts.Token);
                    _logger.LogInformation("Completed {OperationName}", operationName);
                    return result;
                }
                catch (OperationCanceledException)
                {
                    await _cmsUow.RollbackAsync();
                    _logger.LogError("Operation {OperationName} timed out after {Timeout} seconds", operationName, timeoutSeconds ?? DEFAULT_TIMEOUT_SECONDS);
                    throw new McpException($"timed out after {timeoutSeconds ?? DEFAULT_TIMEOUT_SECONDS} seconds");
                }
                catch (Exception ex)
                {
                    await _cmsUow.RollbackAsync();
                    _logger.LogError(ex, "Error in {OperationName}: {ErrorMessage}", operationName, ex.Message);
                    throw new McpException($"{ex.Message}", ex);
                }
            }
        }

        protected async Task ExecuteWithExceptionHandlingAsync(Func<CancellationToken, Task> action, string operationName, int? timeoutSeconds = null)
        {
            using var cts = new CancellationTokenSource(timeoutSeconds.HasValue ? TimeSpan.FromSeconds(timeoutSeconds.Value) : TimeSpan.FromSeconds(DEFAULT_TIMEOUT_SECONDS));
            try
            {
                _logger.LogInformation("Starting {OperationName}", operationName);
                await action(cts.Token);
                await _cmsUow.CompleteAsync(cts.Token);
                _logger.LogInformation("Completed {OperationName}", operationName);
            }
            catch (OperationCanceledException)
            {
                await _cmsUow.RollbackAsync();
                _logger.LogError("Operation {OperationName} timed out after {Timeout} seconds", operationName, timeoutSeconds ?? DEFAULT_TIMEOUT_SECONDS);
                throw new McpException($"timed out after {timeoutSeconds ?? DEFAULT_TIMEOUT_SECONDS} seconds");
            }
            catch (Exception ex)
            {
                await _cmsUow.RollbackAsync();
                _logger.LogError(ex, "Error in {OperationName}: {ErrorMessage}", operationName, ex.Message);
                throw new McpException($"{ex.Message}", ex);
            }
        }
    }
}