using Microsoft.EntityFrameworkCore;
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

        public virtual DbSet<MixCulture> MixCultures { get; set; }
        public virtual DbSet<MixPage> MixPages { get; set; }
        public virtual DbSet<MixPageContent> MixPageContents { get; set; }
        public virtual DbSet<MixPost> MixPosts { get; set; }
        public virtual DbSet<MixPostContent> MixPostContents { get; set; }
        public virtual DbSet<MixConfiguration> MixConfigurations { get; set; }
        public virtual DbSet<MixConfigurationContent> MixConfigurationContents { get; set; }
        public virtual DbSet<MixLanguage> MixLanguages { get; set; }
        public virtual DbSet<MixLanguageContent> MixLanguageContents { get; set; }
        public virtual DbSet<MixDatabase> MixDatabases { get; set; }
        public virtual DbSet<MixDatabaseColumn> MixDatabaseColumns { get; set; }
    }
}
