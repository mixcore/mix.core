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
        Task<DataTable> GetTableColumnsAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableForeignKeysAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableIndexesAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableConstraintsAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableTriggersAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTablePrivilegesAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetDatabaseSizeAsync(CancellationToken cancellationToken = default);
        Task<DataTable> GetTableSizeAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableRowCountAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTablePartitionsAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableCollationAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableEngineAsync(string tableName, CancellationToken cancellationToken = default);
        Task<DataTable> GetTableCreateScriptAsync(string tableName, CancellationToken cancellationToken = default);
    }

    public enum DatabaseType
    {
        MySQL,
        PostgreSQL,
        SQLServer,
        SQLite
    }
}