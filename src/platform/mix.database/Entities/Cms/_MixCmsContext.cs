using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Mix.Database.Services;

using MySqlConnector;

namespace Mix.Database.Entities.Cms
{
    public class MixCmsContext : BaseDbContext
    {
        public IHttpContextAccessor _httpContextAccessor;
        // For Unit Test
        public MixCmsContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
            _dbContextType = GetType();
        }

        public MixCmsContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_CMS_CONNECTION)
        {
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
        public virtual DbSet<MixMedia> MixMedia { get; set; }
        public virtual DbSet<MixApplication> MixApplication { get; set; }
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
        public virtual DbSet<MixDatabaseContext> MixDatabaseContext { get; set; }
        public virtual DbSet<MixDatabase> MixDatabase { get; set; }
        public virtual DbSet<MixDatabaseRelationship> MixDatabaseRelationship { get; set; }
        public virtual DbSet<MixTheme> MixTheme { get; set; }
        public virtual DbSet<MixTemplate> MixViewTemplate { get; set; }
        public virtual DbSet<MixConfigurationContent> MixConfigurationContent { get; set; }
        public virtual DbSet<MixLanguageContent> MixLanguageContent { get; set; }
        public virtual DbSet<MixDatabaseColumn> MixDatabaseColumn { get; set; }
        public virtual DbSet<MixDatabaseAssociation> MixDatabaseAssociation { get; set; }
        public virtual DbSet<MixDatabaseContextDatabaseAssociation> MixDatabaseContextDatabaseAssociation { get; set; }
        public virtual DbSet<MixPagePostAssociation> MixPagePostAssociation { get; set; }
        public virtual DbSet<MixPostPostAssociation> MixPostPostAssociation { get; set; }
        public virtual DbSet<MixPageModuleAssociation> MixPageModuleAssociation { get; set; }
        public virtual DbSet<MixModulePostAssociation> MixModulePostAssociation { get; set; }
        public virtual DbSet<MixContributor> MixContributor { get; set; }

    }
}
