using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Base
{
    public abstract class AccountEntityBaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : class
    {
        protected IDatabaseConstants Config { get; set; }

        protected readonly DatabaseService DatabaseService;

        protected AccountEntityBaseConfiguration(DatabaseService databaseService)
        {
            DatabaseService = databaseService;
            Config = GetConfig();
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {

        }

        private IDatabaseConstants GetConfig()
        {
            switch (DatabaseService.DatabaseProvider)
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
