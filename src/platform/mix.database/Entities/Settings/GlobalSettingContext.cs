using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Cms.EntityConfigurations;
using Mix.Database.Services;
using Mix.Lib.Extensions;
using MySqlConnector;

namespace Mix.Database.Entities.Settings
{
    public class GlobalSettingContext : DbContext
    {

        protected static string _connectionString;
        protected static MixDatabaseProvider _databaseProvider;
        // For Unit Test
        public GlobalSettingContext(IConfiguration configuration) : base()
        {
            _databaseProvider = Enum.Parse<MixDatabaseProvider>(configuration["DatabaseProvider"] ?? "SQLITE");
            _connectionString = configuration.SettingsConnection();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_databaseProvider)
            {
                case MixDatabaseProvider.SQLSERVER:
                    optionsBuilder.UseSqlServer(_connectionString);
                    break;

                case MixDatabaseProvider.MySQL:
                    optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString),
                        b => b.UseMicrosoftJson(MySqlCommonJsonChangeTrackingOptions.FullHierarchyOptimizedFast));
                    break;

                case MixDatabaseProvider.SQLITE:
                    var c = new SqliteConnection(_connectionString);
                    // SQLite not works with guid lower case
                    c.CreateFunction("newid", () => Guid.NewGuid().ToString().ToUpper());
                    optionsBuilder.UseSqlite(c);
                    break;

                case MixDatabaseProvider.PostgreSQL:
                    optionsBuilder.UseNpgsql(_connectionString);
                    break;

                default:
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MixGlobalSettingConfiguration(_databaseProvider));
        }
        public virtual DbSet<MixGlobalSetting> MixGlobalSetting { get; set; }

    }
    public class MixGlobalSettingConfiguration : IEntityTypeConfiguration<MixGlobalSetting>

    {
        protected virtual IDatabaseConstants Config { get; set; }
        protected readonly MixDatabaseProvider _databaseProvider;
        public MixGlobalSettingConfiguration(MixDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            Config = GetConfig();
        }

        public void Configure(EntityTypeBuilder<MixGlobalSetting> builder)
        {
            string key = $"PK_{typeof(MixGlobalSetting).Name}";

            builder.ToTable("mix_global_setting");

            builder.HasKey(e => new { e.Id })
                   .HasName(key);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.TenantId)
                .HasColumnName("tenant_id");

            builder.Property(e => e.IsEncrypt)
                .HasColumnName("is_encrypt");

            builder.Property(e => e.ServiceName)
               .IsRequired()
               .HasColumnName("service_name")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SectionName)
                .HasColumnName("section_name")
              .HasColumnType($"{Config.NString}{Config.MediumLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DisplayName)
              .IsRequired()
              .HasColumnName("display_name")
              .HasColumnType($"{Config.NString}{Config.MediumLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Description)
                .HasColumnName("description")
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.LastModified)
                .HasColumnName("last_modified")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Settings)
                .HasColumnName("settings")
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.SystemName)
                .HasColumnName("system_name")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
        }

        private IDatabaseConstants GetConfig()
        {
            switch (_databaseProvider)
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
