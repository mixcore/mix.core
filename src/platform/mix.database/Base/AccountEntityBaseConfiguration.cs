using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.EntityConfigurations.POSTGRES;
using Mix.Database.EntityConfigurations.SQLITE;
using Mix.Database.EntityConfigurations.SQLSERVER;
using Mix.Database.Services;

namespace Mix.Database.Base
{
    public abstract class AccountEntityBaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : class
    {
        protected virtual IDatabaseConstants Config { get; set; }

        private readonly DatabaseService _databaseService;

        protected AccountEntityBaseConfiguration(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Config = GetConfig();
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            
        }

        private IDatabaseConstants GetConfig()
        {
            switch (_databaseService.DatabaseProvider)
            {
                case MixDatabaseProvider.SQLSERVER:
                    return new SqlServerDatabaseConstants();
                case MixDatabaseProvider.MySQL:
                    return new MySqlDatabaseConstants();
                case MixDatabaseProvider.PostgreSQL:
                    return new PostgresDatabaseConstants();
                case MixDatabaseProvider.SQLITE:
                    return new SqliteDatabaseConstants();
                default: return null;
            }
        }
    }
}
