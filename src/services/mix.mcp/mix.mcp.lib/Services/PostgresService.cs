using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace Mix.MCP.Lib.Services
{
    public interface IPostgresService
    {
        Task<DataTable> ExecuteQueryAsync(string query, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetTableNamesAsync(CancellationToken cancellationToken = default);
        Task<DataTable> GetTableSchemaAsync(string tableName, CancellationToken cancellationToken = default);
    }

    public class PostgresService : IPostgresService
    {
        private readonly string _connectionString;
        private readonly ILogger<PostgresService> _logger;

        public PostgresService(string connectionString, ILogger<PostgresService> logger)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DataTable> ExecuteQueryAsync(string query, CancellationToken cancellationToken = default)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new NpgsqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
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
            const string query = @"
                SELECT column_name, data_type, is_nullable, column_default
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
    }
}