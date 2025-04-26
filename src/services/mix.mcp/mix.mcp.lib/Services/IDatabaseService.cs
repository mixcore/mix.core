using System.Data;
using Mix.MCP.Lib.Models;

namespace Mix.MCP.Lib.Services
{
    public interface IDatabaseService
    {
        Task<IEnumerable<TableSchema>> GetTableSchemasAsync();
        Task<DataTable> ExecuteQueryAsync(string query);
        Task<bool> TestConnectionAsync();
        Task<IEnumerable<string>> GetTableNamesAsync(CancellationToken cancellationToken = default);
        Task<DataTable> GetTableSchemaAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableDataAsync(string tableName, int limit = 100, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableRelationshipsAsync(string tableName, CancellationToken cancellationToken = default);
    }

    public enum DatabaseType
    {
        MySQL,
        PostgreSQL
    }
} 