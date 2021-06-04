using Microsoft.EntityFrameworkCore;
using Mix.Database.Extensions;
using Mix.Heart.Enums;
using Mix.Shared.Constants;
using Mix.Shared.Services;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixCmsContextV2 : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string cnn1 = "Server=localhost;Database=mixcore_structure;UID=tinku;Pwd=1234qwe@;MultipleActiveResultSets=true;";
            optionsBuilder.UseSqlServer(cnn1);
            return;

            string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                var provider = MixAppSettingService.GetEnumConfig<MixDatabaseProvider>(MixConstants.CONST_SETTING_DATABASE_PROVIDER);
                switch (provider)
                {
                    case MixDatabaseProvider.MSSQL:
                        optionsBuilder.UseSqlServer(cnn);
                        break;

                    case MixDatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(cnn, ServerVersion.AutoDetect(cnn));
                        break;

                    case MixDatabaseProvider.SQLITE:
                        optionsBuilder.UseSqlite(cnn);
                        break;

                    case MixDatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(cnn);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "Mix.Database.EntityConfigurations.v2.SQLSERVER");
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
    }
}
