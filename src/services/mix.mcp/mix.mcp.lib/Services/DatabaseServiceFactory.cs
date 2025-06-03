using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.Heart.Enums;

namespace Mix.MCP.Lib.Services
{
    public class DatabaseServiceFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public DatabaseServiceFactory(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public IDatabaseService CreateService()
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration is not initialized");
            }

            var databaseType = _configuration.GetValue<MixDatabaseProvider>("DatabaseProvider");
            var connectionString = _configuration.GetConnectionString("SettingsConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured");
            }

            return databaseType switch
            {
                MixDatabaseProvider.MySQL => new MySqlService(connectionString, _loggerFactory.CreateLogger<MySqlService>()),
                MixDatabaseProvider.PostgreSQL => new PostgreSqlService(connectionString, _loggerFactory.CreateLogger<PostgreSqlService>()),
                MixDatabaseProvider.SQLSERVER => new SqlServerService(connectionString, _loggerFactory.CreateLogger<SqlServerService>()),
                MixDatabaseProvider.SQLITE => new SqliteService(connectionString, _loggerFactory.CreateLogger<SqliteService>()),
                _ => throw new ArgumentException($"Unsupported database type: {databaseType}")
            };
        }
    }
}