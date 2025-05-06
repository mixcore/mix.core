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

        public async Task<DataTable> GetTableColumnsAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    column_name,
                    data_type,
                    character_maximum_length,
                    numeric_precision,
                    numeric_scale,
                    is_nullable,
                    column_default,
                    column_comment,
                    extra
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

        public async Task<DataTable> GetTableForeignKeysAsync(string tableName, CancellationToken cancellationToken = default)
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
                    rc.constraint_name,
                    rc.update_rule,
                    rc.delete_rule,
                    kcu.ordinal_position
                FROM information_schema.key_column_usage kcu
                JOIN information_schema.referential_constraints rc
                    ON kcu.constraint_name = rc.constraint_name
                WHERE kcu.table_schema = DATABASE()
                AND kcu.table_name = @tableName
                AND kcu.referenced_table_name IS NOT NULL
                ORDER BY kcu.ordinal_position";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableIndexesAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    index_name,
                    column_name,
                    non_unique,
                    seq_in_index,
                    collation,
                    cardinality,
                    sub_part,
                    packed,
                    nullable,
                    index_type,
                    comment
                FROM information_schema.statistics
                WHERE table_schema = DATABASE()
                AND table_name = @tableName
                ORDER BY index_name, seq_in_index";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableConstraintsAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    tc.constraint_name,
                    tc.constraint_type,
                    kcu.column_name,
                    kcu.referenced_table_name,
                    kcu.referenced_column_name,
                    rc.update_rule,
                    rc.delete_rule
                FROM information_schema.table_constraints tc
                LEFT JOIN information_schema.key_column_usage kcu
                    ON tc.constraint_name = kcu.constraint_name
                LEFT JOIN information_schema.referential_constraints rc
                    ON tc.constraint_name = rc.constraint_name
                WHERE tc.table_schema = DATABASE()
                AND tc.table_name = @tableName
                ORDER BY tc.constraint_name, kcu.ordinal_position";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableTriggersAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    trigger_name,
                    event_manipulation,
                    event_object_table,
                    action_statement,
                    action_timing,
                    created,
                    sql_mode,
                    definer,
                    character_set_client,
                    collation_connection,
                    database_collation
                FROM information_schema.triggers
                WHERE trigger_schema = DATABASE()
                AND event_object_table = @tableName
                ORDER BY trigger_name";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTablePrivilegesAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    grantee,
                    table_schema,
                    table_name,
                    privilege_type,
                    is_grantable
                FROM information_schema.table_privileges
                WHERE table_schema = DATABASE()
                AND table_name = @tableName
                ORDER BY grantee, privilege_type";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetDatabaseSizeAsync(CancellationToken cancellationToken = default)
        {
            const string query = @"
                SELECT 
                    table_schema AS database_name,
                    ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS size_mb
                FROM information_schema.tables
                WHERE table_schema = DATABASE()
                GROUP BY table_schema";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableSizeAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    table_name,
                    table_rows,
                    ROUND(data_length / 1024 / 1024, 2) AS data_size_mb,
                    ROUND(index_length / 1024 / 1024, 2) AS index_size_mb,
                    ROUND((data_length + index_length) / 1024 / 1024, 2) AS total_size_mb
                FROM information_schema.tables
                WHERE table_schema = DATABASE()
                AND table_name = @tableName";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableRowCountAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            var query = $"SELECT COUNT(*) as row_count FROM {tableName}";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTablePartitionsAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    partition_name,
                    partition_method,
                    partition_expression,
                    partition_description,
                    table_rows,
                    avg_row_length,
                    data_length,
                    create_time,
                    update_time
                FROM information_schema.partitions
                WHERE table_schema = DATABASE()
                AND table_name = @tableName
                ORDER BY partition_ordinal_position";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableCollationAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    table_collation,
                    table_name,
                    table_schema
                FROM information_schema.tables
                WHERE table_schema = DATABASE()
                AND table_name = @tableName";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableEngineAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    engine,
                    version,
                    row_format,
                    table_rows,
                    avg_row_length,
                    data_length,
                    max_data_length,
                    index_length,
                    data_free,
                    auto_increment,
                    create_time,
                    update_time,
                    check_time,
                    table_collation,
                    checksum,
                    create_options,
                    table_comment
                FROM information_schema.tables
                WHERE table_schema = DATABASE()
                AND table_name = @tableName";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<DataTable> GetTableCreateScriptAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = "SHOW CREATE TABLE @tableName";

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