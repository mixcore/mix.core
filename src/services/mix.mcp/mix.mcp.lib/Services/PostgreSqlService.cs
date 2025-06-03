using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Models;
using Npgsql;
using System.Data;

namespace Mix.MCP.Lib.Services
{
    public class PostgreSqlService : IDatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<PostgreSqlService> _logger;

        public PostgreSqlService(string connectionString, ILogger<PostgreSqlService> logger)
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

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to PostgreSQL");
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetTableNamesAsync(CancellationToken cancellationToken = default)
        {
            const string query = @"
                SELECT table_name 
                FROM information_schema.tables 
                WHERE table_schema = 'public' 
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
                SELECT 
                    column_name,
                    data_type,
                    is_nullable,
                    column_default
                FROM information_schema.columns
                WHERE table_schema = 'public'
                AND table_name = @tableName
                ORDER BY ordinal_position";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    tc.constraint_name,
                    kcu.column_name,
                    ccu.table_name AS foreign_table_name,
                    ccu.column_name AS foreign_column_name
                FROM information_schema.table_constraints AS tc
                JOIN information_schema.key_column_usage AS kcu
                    ON tc.constraint_name = kcu.constraint_name
                JOIN information_schema.constraint_column_usage AS ccu
                    ON ccu.constraint_name = tc.constraint_name
                WHERE tc.constraint_type = 'FOREIGN KEY'
                AND tc.table_name = @tableName";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    c.column_name,
                    c.data_type,
                    c.character_maximum_length,
                    c.numeric_precision,
                    c.numeric_scale,
                    c.is_nullable,
                    c.column_default,
                    pgd.description AS column_comment,
                    c.is_identity,
                    c.is_generated
                FROM information_schema.columns c
                LEFT JOIN pg_catalog.pg_statio_all_tables st ON st.schemaname = c.table_schema AND st.relname = c.table_name
                LEFT JOIN pg_catalog.pg_description pgd ON pgd.objoid = st.relid AND pgd.objsubid = c.ordinal_position
                WHERE c.table_name = @tableName
                ORDER BY c.ordinal_position";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    tc.constraint_name,
                    kcu.column_name,
                    ccu.table_name AS referenced_table_name,
                    ccu.column_name AS referenced_column_name,
                    rc.delete_rule,
                    rc.update_rule
                FROM information_schema.table_constraints tc
                JOIN information_schema.key_column_usage kcu
                    ON tc.constraint_name = kcu.constraint_name
                    AND tc.table_schema = kcu.table_schema
                JOIN information_schema.referential_constraints rc
                    ON tc.constraint_name = rc.constraint_name
                    AND tc.table_schema = rc.constraint_schema
                JOIN information_schema.constraint_column_usage ccu
                    ON ccu.constraint_name = tc.constraint_name
                    AND ccu.table_schema = tc.table_schema
                WHERE tc.constraint_type = 'FOREIGN KEY'
                AND tc.table_name = @tableName
                ORDER BY tc.constraint_name, kcu.ordinal_position";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    i.relname AS index_name,
                    a.attname AS column_name,
                    ix.indisunique AS is_unique,
                    ix.indisprimary AS is_primary_key,
                    ix.indisunique AS is_unique_constraint,
                    a.attnum AS key_ordinal,
                    ix.indoption[a.attnum-1] & 1 = 1 AS is_descending_key,
                    am.amname AS index_type,
                    ix.indfillfactor AS fill_factor,
                    ix.indisclustered AS is_clustered,
                    ix.indisvalid AS is_valid,
                    ix.indpred IS NOT NULL AS has_filter,
                    pg_get_expr(ix.indpred, ix.indrelid) AS filter_definition
                FROM pg_class t
                JOIN pg_class i ON i.oid = ANY(pg_index.indrelid)
                JOIN pg_index ix ON ix.indexrelid = i.oid
                JOIN pg_attribute a ON a.attrelid = t.oid AND a.attnum = ANY(ix.indkey)
                JOIN pg_am am ON i.relam = am.oid
                WHERE t.relname = @tableName
                ORDER BY i.relname, a.attnum";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    ccu.table_name AS referenced_table_name,
                    ccu.column_name AS referenced_column_name,
                    rc.delete_rule,
                    rc.update_rule
                FROM information_schema.table_constraints tc
                LEFT JOIN information_schema.key_column_usage kcu
                    ON tc.constraint_name = kcu.constraint_name
                    AND tc.table_schema = kcu.table_schema
                LEFT JOIN information_schema.referential_constraints rc
                    ON tc.constraint_name = rc.constraint_name
                    AND tc.table_schema = rc.constraint_schema
                LEFT JOIN information_schema.constraint_column_usage ccu
                    ON ccu.constraint_name = tc.constraint_name
                    AND ccu.table_schema = tc.table_schema
                WHERE tc.table_name = @tableName
                ORDER BY tc.constraint_type, tc.constraint_name";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    t.tgname AS trigger_name,
                    CASE t.tgtype & 1
                        WHEN 1 THEN 'ROW'
                        ELSE 'STATEMENT'
                    END AS trigger_type,
                    CASE t.tgtype & 2
                        WHEN 2 THEN 'BEFORE'
                        ELSE 'AFTER'
                    END AS trigger_timing,
                    CASE t.tgtype & 4
                        WHEN 4 THEN 'INSERT'
                        WHEN 8 THEN 'DELETE'
                        WHEN 16 THEN 'UPDATE'
                    END AS trigger_event,
                    pg_get_triggerdef(t.oid) AS trigger_definition
                FROM pg_trigger t
                JOIN pg_class c ON t.tgrelid = c.oid
                WHERE c.relname = @tableName
                AND NOT t.tgisinternal
                ORDER BY t.tgname";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    table_name,
                    privilege_type,
                    is_grantable
                FROM information_schema.table_privileges
                WHERE table_name = @tableName
                ORDER BY grantee, privilege_type";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    current_database() AS database_name,
                    pg_size_pretty(pg_database_size(current_database())) AS size,
                    pg_database_size(current_database()) AS size_bytes";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    relname AS table_name,
                    n_live_tup AS row_count,
                    pg_size_pretty(pg_total_relation_size(relid)) AS total_size,
                    pg_size_pretty(pg_relation_size(relid)) AS table_size,
                    pg_size_pretty(pg_total_relation_size(relid) - pg_relation_size(relid)) AS index_size,
                    pg_total_relation_size(relid) AS total_size_bytes,
                    pg_relation_size(relid) AS table_size_bytes,
                    pg_total_relation_size(relid) - pg_relation_size(relid) AS index_size_bytes
                FROM pg_catalog.pg_statio_user_tables
                WHERE relname = @tableName";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                SELECT n_live_tup AS row_count
                FROM pg_catalog.pg_stat_user_tables
                WHERE relname = @tableName";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    c.relname AS partition_name,
                    pg_get_expr(c.relpartbound, c.oid) AS partition_bound,
                    pg_size_pretty(pg_relation_size(c.oid)) AS partition_size,
                    pg_relation_size(c.oid) AS partition_size_bytes,
                    c.reltuples AS estimated_row_count
                FROM pg_class c
                JOIN pg_inherits i ON i.inhrelid = c.oid
                JOIN pg_class p ON p.oid = i.inhparent
                WHERE p.relname = @tableName
                ORDER BY c.relname";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    c.relname AS table_name,
                    d.collname AS collation_name
                FROM pg_class c
                JOIN pg_collation d ON d.oid = c.relcollation
                WHERE c.relname = @tableName";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                    c.relname AS table_name,
                    c.reltuples AS estimated_row_count,
                    pg_size_pretty(pg_total_relation_size(c.oid)) AS total_size,
                    pg_total_relation_size(c.oid) AS total_size_bytes,
                    c.reloptions AS table_options,
                    c.relpersistence AS persistence_type,
                    c.relkind AS relation_type,
                    c.relhasindex AS has_indexes,
                    c.relhasrules AS has_rules,
                    c.relhastriggers AS has_triggers,
                    c.relrowsecurity AS row_security_enabled
                FROM pg_class c
                WHERE c.relname = @tableName";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
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
                SELECT pg_get_ddl(@tableName) AS create_script";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}