using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Extensions;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class SimpleEntityBaseConfiguration<T, TPrimaryKey> : IEntityTypeConfiguration<T>
        where TPrimaryKey : IComparable
        where T : SimpleEntityBase<TPrimaryKey>
    {
        protected virtual IDatabaseConstants Config { get; set; }

        protected readonly DatabaseService _databaseService;

        protected SimpleEntityBaseConfiguration(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Config = GetConfig();
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            string tableName = typeof(T).Name.ToHyphenCase('_', true);
            builder.ToTable(tableName);
            builder.HasKey(e => new { e.Id })
                   .HasName($"pk_{tableName}");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseDefaultGUIDIf(typeof(TPrimaryKey) == typeof(Guid), Config.GenerateUUID)
                .UseIncreaseValueIf(typeof(TPrimaryKey) == typeof(int));
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
