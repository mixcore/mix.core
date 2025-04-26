using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public abstract class BaseMcpTool
    {
        protected readonly ILogger _logger;

        protected BaseMcpTool(ILogger logger)
        {
            _logger = logger;
        }

        protected async Task<T> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> action, string operationName)
        {
            try
            {
                _logger.LogInformation("Starting {OperationName}", operationName);
                var result = await action();
                _logger.LogInformation("Completed {OperationName}", operationName);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {OperationName}: {ErrorMessage}", operationName, ex.Message);
                throw new McpToolException($"Error in {operationName}: {ex.Message}", ex);
            }
        }

        protected async Task ExecuteWithExceptionHandlingAsync(Func<Task> action, string operationName)
        {
            try
            {
                _logger.LogInformation("Starting {OperationName}", operationName);
                await action();
                _logger.LogInformation("Completed {OperationName}", operationName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {OperationName}: {ErrorMessage}", operationName, ex.Message);
                throw new McpToolException($"Error in {operationName}: {ex.Message}", ex);
            }
        }
    }

    public class McpToolException : Exception
    {
        public McpToolException(string message) : base(message)
        {
        }

        public McpToolException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 