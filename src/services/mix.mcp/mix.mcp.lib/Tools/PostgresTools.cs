using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public class PostgresTools
    {
        private readonly IPostgresService _postgresService;
        private readonly ILogger<PostgresTools> _logger;

        public PostgresTools(IPostgresService postgresService, ILogger<PostgresTools> logger)
        {
            _postgresService = postgresService;
            _logger = logger;
        }

        [McpServerTool, Description("Execute a read-only SQL LLMMessage")]
        public async Task<DataTable> ExecuteQueryAsync(
            [Description("SQL LLMMessage to execute")] string message,
            CancellationToken cancellationToken = default)
        {
            // Validate LLMMessage is read-only
            if (message.TrimStart().StartsWith("INSERT", StringComparison.OrdinalIgnoreCase) ||
                message.TrimStart().StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) ||
                message.TrimStart().StartsWith("DELETE", StringComparison.OrdinalIgnoreCase) ||
                message.TrimStart().StartsWith("DROP", StringComparison.OrdinalIgnoreCase) ||
                message.TrimStart().StartsWith("CREATE", StringComparison.OrdinalIgnoreCase) ||
                message.TrimStart().StartsWith("ALTER", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Only read-only queries are allowed");
            }

            return await _postgresService.ExecuteQueryAsync(message, cancellationToken);
        }

        [McpServerTool, Description("Get list of available tables")]
        public async Task<IEnumerable<string>> GetTablesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _postgresService.GetTableNamesAsync(cancellationToken);
        }

        [McpServerTool, Description("Get schema information for a table")]
        public async Task<DataTable> GetTableSchemaAsync(
            [Description("Name of the table")] string message,
            CancellationToken cancellationToken = default)
        {
            return await _postgresService.GetTableSchemaAsync(message, cancellationToken);
        }
    }
}