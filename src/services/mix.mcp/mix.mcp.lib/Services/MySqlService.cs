using System.Data;
using MySql.Data.MySqlClient;
using Mix.MCP.Lib.Models;
using Microsoft.Extensions.Logging;

namespace Mix.MCP.Lib.Services
{
    public class MySqlService : IDatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<MySqlService> _logger;

        public MySqlService(string connectionString, ILogger<MySqlService> logger)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TableSchema>> GetTableSchemasAsync()
        {
            var tableNames = await GetTableNamesAsync();
            var schemas = new List<TableSchema>();

            foreach (var tableName in tableNames)
            {
                var schema = await GetTableSchemaAsync(tableName);
                schemas.Add(new TableSchema
                {
                    TableName = tableName,
                    Columns = schema.AsEnumerable().Select(row => new ColumnSchema
                    {
                        Name = row.Field<string>("column_name") ?? string.Empty,
                        DataType = row.Field<string>("data_type") ?? string.Empty,
                        IsNullable = row.Field<string>("is_nullable") == "YES",
                        DefaultValue = row.Field<string>("column_default")
                    }).ToList()
                });
            }

            return schemas;
        }

        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("Query cannot be null or empty", nameof(query));
            }

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to MySQL");
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetTableNamesAsync(CancellationToken cancellationToken = default)
        {
            const string query = @"
                SELECT table_name 
                FROM information_schema.tables 
                WHERE table_schema = DATABASE() 
                AND table_type = 'BASE TABLE'";

            var result = await ExecuteQueryAsync(query);
            return result.AsEnumerable()
                .Select(row => row.Field<string>("table_name") ?? string.Empty)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList();
        }

        public async Task<DataTable> GetTableSchemaAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT column_name, data_type, is_nullable, column_default
                FROM information_schema.columns
                WHERE table_schema = DATABASE()
                AND table_name = @tableName
                ORDER BY ordinal_position";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableDataAsync(string tableName, int limit = 100, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            var query = $"SELECT * FROM {tableName} LIMIT {limit}";
            return await ExecuteQueryAsync(query);
        }

        public async Task<DataTable> GetTableRelationshipsAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    kcu.column_name,
                    kcu.referenced_table_name,
                    kcu.referenced_column_name,
                    rc.update_rule,
                    rc.delete_rule
                FROM information_schema.key_column_usage kcu
                JOIN information_schema.referential_constraints rc
                    ON kcu.constraint_name = rc.constraint_name
                WHERE kcu.table_schema = DATABASE()
                AND kcu.table_name = @tableName";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}