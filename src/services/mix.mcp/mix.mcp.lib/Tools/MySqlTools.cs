using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Models;
using Mix.MCP.Lib.Services;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public class MySqlTools : BaseMcpTool
    {
        private readonly IDatabaseService _databaseService;

        public MySqlTools(IDatabaseService databaseService, ILogger<MySqlTools> logger) : base(logger)
        {
            _databaseService = databaseService;
        }

        [McpServerTool, Description("Execute a read-only SQL query")]
        public async Task<DataTableModel> ExecuteQueryAsync(string query)
        {
            return await ExecuteWithExceptionHandlingAsync(async () =>
            {
                if (string.IsNullOrEmpty(query))
                {
                    throw new ArgumentException("Query cannot be empty");
                }

                var result = await _databaseService.ExecuteQueryAsync(query);
                return DataTableModel.FromDataTable(result);
            }, "ExecuteQuery");
        }

        [McpServerTool, Description("Get list of available tables")]
        public async Task<IEnumerable<string>> GetTablesAsync()
        {
            return await ExecuteWithExceptionHandlingAsync(
                () => _databaseService.GetTableNamesAsync(),
                "GetTables");
        }

        [McpServerTool, Description("Get schema information for a table")]
        public async Task<DataTableModel> GetTableSchemaAsync(string tableName)
        {
            return await ExecuteWithExceptionHandlingAsync(async () =>
            {
                var result = await _databaseService.GetTableSchemaAsync(tableName);
                return DataTableModel.FromDataTable(result);
            }, "GetTableSchema");
        }

        [McpServerTool, Description("Get sample data from a table")]
        public async Task<DataTableModel> GetTableDataAsync(string tableName, int limit = 100)
        {
            return await ExecuteWithExceptionHandlingAsync(async () =>
            {
                var result = await _databaseService.GetTableDataAsync(tableName, limit);
                return DataTableModel.FromDataTable(result);
            }, "GetTableData");
        }

        [McpServerTool, Description("Get relationships for a table")]
        public async Task<DataTableModel> GetTableRelationshipsAsync(string tableName)
        {
            return await ExecuteWithExceptionHandlingAsync(async () =>
            {
                var result = await _databaseService.GetTableRelationshipsAsync(tableName);
                return DataTableModel.FromDataTable(result);
            }, "GetTableRelationships");
        }

        [McpServerTool, Description("Get database information")]
        public async Task<DataTableModel> GetDatabaseInfoAsync()
        {
            return await ExecuteWithExceptionHandlingAsync(async () =>
            {
                const string query = @"
                    SELECT table_name, table_rows, data_length, index_length
                    FROM information_schema.tables
                    WHERE table_schema = DATABASE()
                    AND table_type = 'BASE TABLE'";

                var result = await _databaseService.ExecuteQueryAsync(query);
                return DataTableModel.FromDataTable(result);
            }, "GetDatabaseInfo");
        }
    }
} 