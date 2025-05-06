using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.MCP.Lib.Models;
using Mix.MCP.Lib.Services;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public class MySqlTools : BaseMcpTool
    {
        private readonly IDatabaseService _databaseService;

        public MySqlTools(IDatabaseService databaseService, UnitOfWorkInfo<MixCmsContext> cmsUow, ILogger<MySqlTools> logger) : base(cmsUow, logger)
        {
            _databaseService = databaseService;
        }

        [McpServerTool, Description("Execute a read-only SQL query")]
        public async Task<DataTableModel> ExecuteQueryAsync(string query)
        {
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
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
                async (ct) => await _databaseService.GetTableNamesAsync(),
                "GetTables");
        }

        [McpServerTool, Description("Get schema information for a table")]
        public async Task<DataTableModel> GetTableSchemaAsync(string tableName)
        {
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new ArgumentException("Table name cannot be empty");
                }

                var result = await _databaseService.GetTableSchemaAsync(tableName);
                return DataTableModel.FromDataTable(result);
            }, "GetTableSchema");
        }

        [McpServerTool, Description("Get column information for a table")]
        public async Task<DataTableModel> GetTableColumnsAsync(string tableName)
        {
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new ArgumentException("Table name cannot be empty");
                }

                var result = await _databaseService.GetTableColumnsAsync(tableName);
                return DataTableModel.FromDataTable(result);
            }, "GetTableColumns");
        }

        [McpServerTool, Description("Get foreign key information for a table")]
        public async Task<DataTableModel> GetTableForeignKeysAsync(string tableName)
        {
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new ArgumentException("Table name cannot be empty");
                }

                var result = await _databaseService.GetTableForeignKeysAsync(tableName);
                return DataTableModel.FromDataTable(result);
            }, "GetTableForeignKeys");
        }

        [McpServerTool, Description("Get index information for a table")]
        public async Task<DataTableModel> GetTableIndexesAsync(string tableName)
        {
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new ArgumentException("Table name cannot be empty");
                }

                var result = await _databaseService.GetTableIndexesAsync(tableName);
                return DataTableModel.FromDataTable(result);
            }, "GetTableIndexes");
        }

        [McpServerTool, Description("Get sample data from a table")]
        public async Task<DataTableModel> GetTableDataAsync(string tableName, int limit = 100)
        {
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new ArgumentException("Table name cannot be empty");
                }

                if (limit <= 0)
                {
                    throw new ArgumentException("Limit must be greater than 0");
                }

                var result = await _databaseService.GetTableDataAsync(tableName, limit);
                return DataTableModel.FromDataTable(result);
            }, "GetTableData");
        }

        [McpServerTool, Description("Get relationships for a table")]
        public async Task<DataTableModel> GetTableRelationshipsAsync(string tableName)
        {
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new ArgumentException("Table name cannot be empty");
                }

                var result = await _databaseService.GetTableRelationshipsAsync(tableName);
                return DataTableModel.FromDataTable(result);
            }, "GetTableRelationships");
        }

        [McpServerTool, Description("Get database information")]
        public async Task<DataTableModel> GetDatabaseInfoAsync()
        {
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
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