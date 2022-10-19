using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.EntityConfigurations.POSTGRES;
using Mix.Database.EntityConfigurations.SQLITE;
using Mix.Database.EntityConfigurations.SQLSERVER;
using Mix.Database.Services;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class EntityBaseConfiguration<T, TPrimaryKey> : IEntityTypeConfiguration<T>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
        protected virtual IDatabaseConstants Config { get; set; }

        private readonly DatabaseService _databaseService;

        protected EntityBaseConfiguration(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            Config = GetConfig();
            string key = $"PK_{typeof(T).Name}";
            builder.HasKey(e => new { e.Id })
                   .HasName(key);

            builder.Property(e => e.Id)
                .UseDefaultGUIDIf(typeof(TPrimaryKey) == typeof(Guid), Config.GenerateUUID)
                .UseIncreaseValueIf(typeof(TPrimaryKey) == typeof(int));


            builder.Property(e => e.CreatedDateTime)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ModifiedBy)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Priority)
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Priority)
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
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
