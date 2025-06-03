using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Models;
using System.Data;

namespace Mix.MCP.Lib.Services
{
    public class SqliteService : IDatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<SqliteService> _logger;

        public SqliteService(string connectionString, ILogger<SqliteService> logger)
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

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqliteCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<IEnumerable<string>> GetTableNamesAsync(CancellationToken cancellationToken = default)
        {
            const string query = "SELECT name FROM sqlite_master WHERE type='table'";

            var result = await ExecuteQueryAsync(query);
            return result.AsEnumerable()
                .Select(row => row.Field<string>("name") ?? string.Empty)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList();
        }

        public async Task<DataTable> GetTableSchemaAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            var query = $"PRAGMA table_info({tableName})";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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

            var query = @"
                SELECT 
                    m.name AS ForeignKeyName,
                    m.tbl_name AS TableName,
                    p.name AS ColumnName,
                    m.references AS ReferencedTableName,
                    p.name AS ReferencedColumnName
                FROM sqlite_master m
                JOIN pragma_foreign_key_list(m.name) p
                WHERE m.type = 'table'
                AND m.tbl_name = @tableName";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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
                        Name = row.Field<string>("name") ?? string.Empty,
                        DataType = row.Field<string>("type") ?? string.Empty,
                        IsNullable = !row.Field<bool>("notnull"),
                        DefaultValue = row.Field<string>("dflt_value")
                    }).ToList()
                });
            }

            return schemas;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to SQLite");
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
                    name AS column_name,
                    type AS data_type,
                    CASE 
                        WHEN type LIKE '%char%' THEN length
                        ELSE NULL 
                    END AS character_maximum_length,
                    CASE 
                        WHEN type LIKE '%int%' THEN 10
                        WHEN type LIKE '%real%' THEN 15
                        ELSE NULL 
                    END AS numeric_precision,
                    CASE 
                        WHEN type LIKE '%real%' THEN 6
                        ELSE NULL 
                    END AS numeric_scale,
                    CASE WHEN notnull = 0 THEN 'YES' ELSE 'NO' END AS is_nullable,
                    dflt_value AS column_default,
                    NULL AS column_comment,
                    CASE WHEN pk = 1 THEN 1 ELSE 0 END AS is_identity,
                    0 AS is_generated
                FROM pragma_table_info(@tableName)
                ORDER BY cid";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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
                    'FK_' || m.name || '_' || p.id AS constraint_name,
                    p.from AS column_name,
                    p.table AS referenced_table_name,
                    p.to AS referenced_column_name,
                    CASE p.on_delete
                        WHEN 'CASCADE' THEN 'CASCADE'
                        WHEN 'SET NULL' THEN 'SET NULL'
                        WHEN 'SET DEFAULT' THEN 'SET DEFAULT'
                        WHEN 'RESTRICT' THEN 'RESTRICT'
                        ELSE 'NO ACTION'
                    END AS delete_rule,
                    CASE p.on_update
                        WHEN 'CASCADE' THEN 'CASCADE'
                        WHEN 'SET NULL' THEN 'SET NULL'
                        WHEN 'SET DEFAULT' THEN 'SET DEFAULT'
                        WHEN 'RESTRICT' THEN 'RESTRICT'
                        ELSE 'NO ACTION'
                    END AS update_rule
                FROM sqlite_master m
                JOIN pragma_foreign_key_list(m.name) p
                WHERE m.type = 'table'
                AND m.name = @tableName
                ORDER BY p.id";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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
                    ii.name AS column_name,
                    i.unique AS is_unique,
                    i.origin = 'pk' AS is_primary_key,
                    i.unique AS is_unique_constraint,
                    ii.seqno AS key_ordinal,
                    ii.desc AS is_descending_key,
                    'BTREE' AS index_type,
                    NULL AS fill_factor,
                    0 AS is_padded,
                    1 AS allow_row_locks,
                    1 AS allow_page_locks,
                    0 AS has_filter,
                    NULL AS filter_definition
                FROM sqlite_master i
                JOIN pragma_index_info(i.name) ii
                WHERE i.type = 'index'
                AND i.tbl_name = @tableName
                ORDER BY i.name, ii.seqno";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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
                    CASE 
                        WHEN i.origin = 'pk' THEN 'PRIMARY KEY'
                        WHEN i.unique = 1 THEN 'UNIQUE'
                        ELSE 'FOREIGN KEY'
                    END AS constraint_type,
                    i.name AS constraint_name,
                    ii.name AS column_name,
                    p.table AS referenced_table_name,
                    p.to AS referenced_column_name,
                    CASE p.on_delete
                        WHEN 'CASCADE' THEN 'CASCADE'
                        WHEN 'SET NULL' THEN 'SET NULL'
                        WHEN 'SET DEFAULT' THEN 'SET DEFAULT'
                        WHEN 'RESTRICT' THEN 'RESTRICT'
                        ELSE 'NO ACTION'
                    END AS delete_rule,
                    CASE p.on_update
                        WHEN 'CASCADE' THEN 'CASCADE'
                        WHEN 'SET NULL' THEN 'SET NULL'
                        WHEN 'SET DEFAULT' THEN 'SET DEFAULT'
                        WHEN 'RESTRICT' THEN 'RESTRICT'
                        ELSE 'NO ACTION'
                    END AS update_rule
                FROM sqlite_master i
                LEFT JOIN pragma_index_info(i.name) ii ON i.type = 'index'
                LEFT JOIN pragma_foreign_key_list(@tableName) p ON i.origin = 'fk'
                WHERE i.tbl_name = @tableName
                AND (i.origin = 'pk' OR i.unique = 1 OR i.origin = 'fk')
                ORDER BY i.origin, i.name";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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
                    name AS trigger_name,
                    CASE 
                        WHEN instr(lower(sql), 'for each row') > 0 THEN 'ROW'
                        ELSE 'STATEMENT'
                    END AS trigger_type,
                    CASE 
                        WHEN instr(lower(sql), 'before') > 0 THEN 'BEFORE'
                        ELSE 'AFTER'
                    END AS trigger_timing,
                    CASE 
                        WHEN instr(lower(sql), 'insert') > 0 THEN 'INSERT'
                        WHEN instr(lower(sql), 'delete') > 0 THEN 'DELETE'
                        WHEN instr(lower(sql), 'update') > 0 THEN 'UPDATE'
                    END AS trigger_event,
                    sql AS trigger_definition
                FROM sqlite_master
                WHERE type = 'trigger'
                AND tbl_name = @tableName
                ORDER BY name";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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

            // SQLite doesn't have a built-in privilege system
            // Return an empty table with the expected columns
            var dataTable = new DataTable();
            dataTable.Columns.Add("grantee", typeof(string));
            dataTable.Columns.Add("table_name", typeof(string));
            dataTable.Columns.Add("privilege_type", typeof(string));
            dataTable.Columns.Add("is_grantable", typeof(bool));

            return dataTable;
        }

        public async Task<DataTable> GetDatabaseSizeAsync(CancellationToken cancellationToken = default)
        {
            const string query = @"
                SELECT 
                    'main' AS database_name,
                    page_count * page_size AS size_bytes,
                    printf('%.2f MB', (page_count * page_size) / (1024.0 * 1024.0)) AS size
                FROM pragma_page_count(), pragma_page_size()";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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
                    name AS table_name,
                    (SELECT COUNT(*) FROM @tableName) AS row_count,
                    (SELECT page_count * page_size FROM pragma_page_count(@tableName), pragma_page_size()) AS total_size_bytes,
                    printf('%.2f MB', (SELECT page_count * page_size FROM pragma_page_count(@tableName), pragma_page_size()) / (1024.0 * 1024.0)) AS total_size,
                    (SELECT page_count * page_size FROM pragma_page_count(@tableName), pragma_page_size()) AS table_size_bytes,
                    printf('%.2f MB', (SELECT page_count * page_size FROM pragma_page_count(@tableName), pragma_page_size()) / (1024.0 * 1024.0)) AS table_size,
                    0 AS index_size_bytes,
                    '0 MB' AS index_size
                FROM sqlite_master
                WHERE type = 'table'
                AND name = @tableName";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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

            var query = $"SELECT COUNT(*) AS row_count FROM {tableName}";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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

            // SQLite doesn't support table partitioning
            // Return an empty table with the expected columns
            var dataTable = new DataTable();
            dataTable.Columns.Add("partition_name", typeof(string));
            dataTable.Columns.Add("partition_bound", typeof(string));
            dataTable.Columns.Add("partition_size", typeof(string));
            dataTable.Columns.Add("partition_size_bytes", typeof(long));
            dataTable.Columns.Add("estimated_row_count", typeof(long));

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
                    name AS table_name,
                    'BINARY' AS collation_name
                FROM sqlite_master
                WHERE type = 'table'
                AND name = @tableName";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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
                    name AS table_name,
                    (SELECT COUNT(*) FROM @tableName) AS estimated_row_count,
                    (SELECT page_count * page_size FROM pragma_page_count(@tableName), pragma_page_size()) AS total_size_bytes,
                    printf('%.2f MB', (SELECT page_count * page_size FROM pragma_page_count(@tableName), pragma_page_size()) / (1024.0 * 1024.0)) AS total_size,
                    'SQLite' AS table_engine,
                    'B-tree' AS storage_engine,
                    sql AS create_statement,
                    CASE 
                        WHEN sql LIKE '%WITHOUT ROWID%' THEN 1
                        ELSE 0
                    END AS is_without_rowid,
                    CASE 
                        WHEN sql LIKE '%STRICT%' THEN 1
                        ELSE 0
                    END AS is_strict
                FROM sqlite_master
                WHERE type = 'table'
                AND name = @tableName";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
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
                SELECT sql AS create_script
                FROM sqlite_master
                WHERE type = 'table'
                AND name = @tableName";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}