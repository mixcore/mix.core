using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Models;
using System.Data;

namespace Mix.MCP.Lib.Services
{
    public class SqlServerService : IDatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlServerService> _logger;

        public SqlServerService(string connectionString, ILogger<SqlServerService> logger)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("Query cannot be null or empty", nameof(query));
            }

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<IEnumerable<string>> GetTableNamesAsync(CancellationToken cancellationToken = default)
        {
            const string query = @"
                SELECT TABLE_NAME 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_TYPE = 'BASE TABLE'";

            var result = await ExecuteQueryAsync(query);
            return result.AsEnumerable()
                .Select(row => row.Field<string>("TABLE_NAME") ?? string.Empty)
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
                SELECT 
                    COLUMN_NAME,
                    DATA_TYPE,
                    IS_NULLABLE,
                    COLUMN_DEFAULT
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @tableName
                ORDER BY ORDINAL_POSITION";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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

            var query = $"SELECT TOP {limit} * FROM {tableName}";
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
                    fk.name AS ForeignKeyName,
                    OBJECT_NAME(fk.parent_object_id) AS TableName,
                    c1.name AS ColumnName,
                    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTableName,
                    c2.name AS ReferencedColumnName
                FROM sys.foreign_keys fk
                INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
                INNER JOIN sys.columns c1 ON fkc.parent_object_id = c1.object_id AND fkc.parent_column_id = c1.column_id
                INNER JOIN sys.columns c2 ON fkc.referenced_object_id = c2.object_id AND fkc.referenced_column_id = c2.column_id
                WHERE OBJECT_NAME(fk.parent_object_id) = @tableName";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
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
                        Name = row.Field<string>("COLUMN_NAME") ?? string.Empty,
                        DataType = row.Field<string>("DATA_TYPE") ?? string.Empty,
                        IsNullable = row.Field<string>("IS_NULLABLE") == "YES",
                        DefaultValue = row.Field<string>("COLUMN_DEFAULT")
                    }).ToList()
                });
            }

            return schemas;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to SQL Server");
                return false;
            }
        }

        public async Task<DataTable> GetTableColumnsAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            const string query = @"
                SELECT 
                    c.name AS column_name,
                    t.name AS data_type,
                    c.max_length,
                    c.precision,
                    c.scale,
                    c.is_nullable,
                    OBJECT_DEFINITION(c.default_object_id) AS column_default,
                    ep.value AS column_comment,
                    c.is_identity,
                    c.is_computed
                FROM sys.columns c
                INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
                LEFT JOIN sys.extended_properties ep ON ep.major_id = c.object_id 
                    AND ep.minor_id = c.column_id 
                    AND ep.name = 'MS_Description'
                WHERE OBJECT_ID(@tableName) = c.object_id
                ORDER BY c.column_id";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    fk.name AS constraint_name,
                    c1.name AS column_name,
                    OBJECT_NAME(fk.referenced_object_id) AS referenced_table_name,
                    c2.name AS referenced_column_name,
                    fk.delete_referential_action_desc AS delete_rule,
                    fk.update_referential_action_desc AS update_rule
                FROM sys.foreign_keys fk
                INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
                INNER JOIN sys.columns c1 ON fkc.parent_object_id = c1.object_id AND fkc.parent_column_id = c1.column_id
                INNER JOIN sys.columns c2 ON fkc.referenced_object_id = c2.object_id AND fkc.referenced_column_id = c2.column_id
                WHERE OBJECT_NAME(fk.parent_object_id) = @tableName
                ORDER BY fk.name, fkc.constraint_column_id";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    i.name AS index_name,
                    c.name AS column_name,
                    i.is_unique,
                    i.is_primary_key,
                    i.is_unique_constraint,
                    ic.key_ordinal,
                    ic.is_descending_key,
                    i.type_desc,
                    i.fill_factor,
                    i.is_padded,
                    i.allow_row_locks,
                    i.allow_page_locks,
                    i.has_filter,
                    i.filter_definition
                FROM sys.indexes i
                INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                WHERE OBJECT_ID(@tableName) = i.object_id
                ORDER BY i.name, ic.key_ordinal";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    OBJECT_NAME(c.object_id) AS constraint_name,
                    c.type_desc AS constraint_type,
                    COL_NAME(c.parent_object_id, c.parent_column_id) AS column_name,
                    OBJECT_NAME(c.referenced_object_id) AS referenced_table_name,
                    COL_NAME(c.referenced_object_id, c.referenced_column_id) AS referenced_column_name,
                    c.delete_referential_action_desc AS delete_rule,
                    c.update_referential_action_desc AS update_rule
                FROM sys.objects c
                WHERE c.parent_object_id = OBJECT_ID(@tableName)
                AND c.type IN ('PK', 'UQ', 'F')
                ORDER BY c.type_desc, c.name";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    t.name AS trigger_name,
                    t.type_desc AS trigger_type,
                    t.is_instead_of_trigger,
                    t.is_first,
                    t.is_last,
                    t.create_date,
                    t.modify_date,
                    OBJECT_DEFINITION(t.object_id) AS trigger_definition
                FROM sys.triggers t
                WHERE t.parent_id = OBJECT_ID(@tableName)
                ORDER BY t.name";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    USER_NAME(grantee_principal_id) AS grantee,
                    OBJECT_NAME(major_id) AS table_name,
                    permission_name AS privilege_type,
                    state_desc AS grant_type
                FROM sys.database_permissions
                WHERE major_id = OBJECT_ID(@tableName)
                AND class = 1
                ORDER BY grantee, permission_name";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    DB_NAME() AS database_name,
                    SUM(size * 8.0 / 1024) AS size_mb
                FROM sys.database_files
                GROUP BY DB_NAME()";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    OBJECT_NAME(p.object_id) AS table_name,
                    p.rows AS row_count,
                    SUM(a.total_pages) * 8.0 / 1024 AS total_size_mb,
                    SUM(a.used_pages) * 8.0 / 1024 AS used_size_mb,
                    (SUM(a.total_pages) - SUM(a.used_pages)) * 8.0 / 1024 AS unused_size_mb
                FROM sys.partitions p
                INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
                WHERE p.object_id = OBJECT_ID(@tableName)
                GROUP BY p.object_id, p.rows";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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

            const string query = @"
                SELECT SUM(p.rows) AS row_count
                FROM sys.partitions p
                WHERE p.object_id = OBJECT_ID(@tableName)
                AND p.index_id IN (0, 1)";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

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
                    p.partition_number,
                    fg.name AS filegroup_name,
                    p.rows AS row_count,
                    au.total_pages * 8.0 / 1024 AS total_size_mb,
                    au.used_pages * 8.0 / 1024 AS used_size_mb,
                    p.data_compression_desc,
                    p.data_compression_level
                FROM sys.partitions p
                INNER JOIN sys.allocation_units au ON p.partition_id = au.container_id
                INNER JOIN sys.filegroups fg ON au.data_space_id = fg.data_space_id
                WHERE p.object_id = OBJECT_ID(@tableName)
                ORDER BY p.partition_number";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    t.name AS table_name,
                    c.name AS collation_name
                FROM sys.tables t
                INNER JOIN sys.columns c ON t.object_id = c.object_id
                WHERE t.object_id = OBJECT_ID(@tableName)
                GROUP BY t.name, c.name";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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
                    t.name AS table_name,
                    p.rows AS row_count,
                    au.total_pages * 8.0 / 1024 AS total_size_mb,
                    au.used_pages * 8.0 / 1024 AS used_size_mb,
                    t.create_date,
                    t.modify_date,
                    t.is_ms_shipped,
                    t.is_published,
                    t.is_schema_published,
                    t.lock_escalation_desc,
                    t.text_in_row_limit,
                    t.large_value_types_out_of_row
                FROM sys.tables t
                INNER JOIN sys.partitions p ON t.object_id = p.object_id
                INNER JOIN sys.allocation_units au ON p.partition_id = au.container_id
                WHERE t.object_id = OBJECT_ID(@tableName)
                GROUP BY t.name, p.rows, au.total_pages, au.used_pages, 
                         t.create_date, t.modify_date, t.is_ms_shipped,
                         t.is_published, t.is_schema_published, t.lock_escalation_desc,
                         t.text_in_row_limit, t.large_value_types_out_of_row";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
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

            const string query = @"
                SELECT OBJECT_DEFINITION(OBJECT_ID(@tableName)) AS create_script";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}