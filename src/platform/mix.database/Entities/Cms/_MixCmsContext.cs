using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.EntityConfigurations.POSTGRES;
using Mix.Database.EntityConfigurations.SQLITE;
using Mix.Database.EntityConfigurations.SQLSERVER;
using Mix.Database.Services;
using Mix.Shared.Constants;
using MySqlConnector;
using System;

namespace Mix.Database.Entities.Cms
{
    public class MixCmsContext : DbContext
    {
        // For Unit Test
        public MixCmsContext(string connectionString, MixDatabaseProvider databaseProvider)
        {
            _connectionString = connectionString;
            _databaseProvider = databaseProvider;
        }
        
        // For Unit Test
        public MixCmsContext()
        {
            _databaseService = new();
            _connectionString = _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            _databaseProvider = _databaseService.DatabaseProvider;
        }

        public MixCmsContext(MixDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _connectionString = _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            _databaseProvider = _databaseService.DatabaseProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                switch (_databaseProvider)
                {
                    case MixDatabaseProvider.SQLSERVER:
                        optionsBuilder.UseSqlServer(_connectionString);
                        break;

                    case MixDatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
                        break;

                    case MixDatabaseProvider.SQLITE:
                        optionsBuilder.UseSqlite(_connectionString);
                        break;

                    case MixDatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(_connectionString);
                        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string ns = _databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER
                     => typeof(SqlServerDatabaseConstants).Namespace,

                MixDatabaseProvider.MySQL
                    => typeof(MySqlDatabaseConstants).Namespace,

                MixDatabaseProvider.SQLITE
                    => typeof(SqliteDatabaseConstants).Namespace,

                MixDatabaseProvider.PostgreSQL
                    => typeof(PostgresDatabaseConstants).Namespace,
                _ => string.Empty
            };
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == ns);
        }
        public override void Dispose()
        {
            switch (_databaseProvider)
            {
                case MixDatabaseProvider.SQLSERVER:
                    SqlConnection.ClearPool((SqlConnection)Database.GetDbConnection());
                    break;

                case MixDatabaseProvider.MySQL:
                    MySqlConnection.ClearPool((MySqlConnection)Database.GetDbConnection());
                    break;
            }
            base.Dispose();
            GC.SuppressFinalize(this);
        }


        public virtual DbSet<MixTenant> MixTenant { get; set; }
        public virtual DbSet<MixDomain> MixDomain { get; set; }
        public virtual DbSet<MixCulture> MixCulture { get; set; }
        public virtual DbSet<MixPage> MixPage { get; set; }
        public virtual DbSet<MixPageContent> MixPageContent { get; set; }
        public virtual DbSet<MixModule> MixModule { get; set; }
        public virtual DbSet<MixModuleContent> MixModuleContent { get; set; }
        public virtual DbSet<MixModuleData> MixModuleData { get; set; }
        public virtual DbSet<MixPost> MixPost { get; set; }
        public virtual DbSet<MixPostContent> MixPostContent { get; set; }
        public virtual DbSet<MixUrlAlias> MixUrlAlias { get; set; }
        public virtual DbSet<MixConfiguration> MixConfiguration { get; set; }
        public virtual DbSet<MixLanguage> MixLanguage { get; set; }
        public virtual DbSet<MixDatabase> MixDatabase { get; set; }
        public virtual DbSet<MixTheme> MixTheme { get; set; }
        public virtual DbSet<MixTemplate> MixViewTemplate { get; set; }
        public virtual DbSet<MixConfigurationContent> MixConfigurationContent { get; set; }
        public virtual DbSet<MixLanguageContent> MixLanguageContent { get; set; }
        public virtual DbSet<MixDatabaseColumn> MixDatabaseColumn { get; set; }
        public virtual DbSet<MixData> MixData { get; set; }
        public virtual DbSet<MixDataContent> MixDataContent { get; set; }
        public virtual DbSet<MixDataContentValue> MixDataContentValue { get; set; }
        public virtual DbSet<MixDataContentAssociation> MixDataContentAssociation { get; set; }
        public virtual DbSet<MixPagePostAssociation> MixPagePostAssociation { get; set; }
        public virtual DbSet<MixPageModuleAssociation> MixPageModuleAssociation { get; set; }
        public virtual DbSet<MixModulePostAssociation> MixModulePostAssociation { get; set; }

        private static string _connectionString;
        private static MixDatabaseProvider _databaseProvider;
        private readonly MixDatabaseService _databaseService;
    }
}
