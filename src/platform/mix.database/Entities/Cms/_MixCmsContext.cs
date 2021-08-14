using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Shared.Constants;
using Mix.Shared.Services;
using MySqlConnector;
using System;

namespace Mix.Database.Entities.Cms
{
    public class MixCmsContext : DbContext
    {
        public MixCmsContext()
        {
        }

        // For Unit Test
        public MixCmsContext(string connectionString, MixDatabaseProvider databaseProvider)
        {
            _connectionString = connectionString;
            _databaseProvider = databaseProvider;
        }

        public MixCmsContext(MixDatabaseService databaseService,
            MixAppSettingService appSettingService)
        {
            _connectionString = databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            _databaseProvider = appSettingService.DatabaseProvider;
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
                        break;

                    default:
                        break;
                }
            }
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


        public virtual DbSet<MixSite> MixSite { get; set; }
        public virtual DbSet<MixDomain> MixDomain { get; set; }
        public virtual DbSet<MixCulture> MixCulture { get; set; }
        public virtual DbSet<MixPage> MixPage { get; set; }
        public virtual DbSet<MixPost> MixPost { get; set; }
        public virtual DbSet<MixUrlAlias> MixUrlAlias { get; set; }
        public virtual DbSet<MixConfiguration> MixConfiguration { get; set; }
        public virtual DbSet<MixLanguage> MixLanguage { get; set; }
        public virtual DbSet<MixDatabase> MixDatabase { get; set; }
        public virtual DbSet<MixTheme> MixTheme { get; set; }
        public virtual DbSet<MixViewTemplate> MixViewTemplate { get; set; }
        public virtual DbSet<MixPageContent> MixPageContent { get; set; }
        public virtual DbSet<MixPostContent> MixPostContent { get; set; }
        public virtual DbSet<MixUrlAliasContent> MixUrlAliasContent { get; set; }
        public virtual DbSet<MixConfigurationContent> MixConfigurationContent { get; set; }
        public virtual DbSet<MixLanguageContent> MixLanguageContent { get; set; }
        public virtual DbSet<MixDatabaseColumn> MixDatabaseColumn { get; set; }
        public virtual DbSet<MixData> MixData { get; set; }
        public virtual DbSet<MixDataContent> MixDataContent { get; set; }
        public virtual DbSet<MixDataContentValue> MixDataContentValue { get; set; }
        public virtual DbSet<MixDataContentAssociation> MixDataContentAssociation { get; set; }

        private static string _connectionString;
        private static MixDatabaseProvider _databaseProvider;
    }
}
