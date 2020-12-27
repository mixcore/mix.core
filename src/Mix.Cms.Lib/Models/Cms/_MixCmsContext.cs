using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Services;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Reflection;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixCmsContext : DbContext
    {
        public virtual DbSet<MixAttributeField> MixAttributeField { get; set; }
        public virtual DbSet<MixAttributeSet> MixAttributeSet { get; set; }
        public virtual DbSet<MixAttributeSetData> MixAttributeSetData { get; set; }
        public virtual DbSet<MixAttributeSetReference> MixAttributeSetReference { get; set; }
        public virtual DbSet<MixAttributeSetValue> MixAttributeSetValue { get; set; }
        public virtual DbSet<MixCache> MixCache { get; set; }
        public virtual DbSet<MixCmsUser> MixCmsUser { get; set; }
        public virtual DbSet<MixConfiguration> MixConfiguration { get; set; }
        public virtual DbSet<MixCulture> MixCulture { get; set; }
        public virtual DbSet<MixFile> MixFile { get; set; }
        public virtual DbSet<MixLanguage> MixLanguage { get; set; }
        public virtual DbSet<MixMedia> MixMedia { get; set; }
        public virtual DbSet<MixModule> MixModule { get; set; }
        public virtual DbSet<MixModuleData> MixModuleData { get; set; }
        public virtual DbSet<MixModulePost> MixModulePost { get; set; }
        public virtual DbSet<MixPage> MixPage { get; set; }
        public virtual DbSet<MixPageModule> MixPageModule { get; set; }
        public virtual DbSet<MixPagePost> MixPagePost { get; set; }
        public virtual DbSet<MixPortalPage> MixPortalPage { get; set; }
        public virtual DbSet<MixPortalPageNavigation> MixPortalPageNavigation { get; set; }
        public virtual DbSet<MixPortalPageRole> MixPortalPageRole { get; set; }
        public virtual DbSet<MixPost> MixPost { get; set; }
        public virtual DbSet<MixPostMedia> MixPostMedia { get; set; }
        public virtual DbSet<MixPostModule> MixPostModule { get; set; }
        public virtual DbSet<MixRelatedAttributeData> MixRelatedAttributeData { get; set; }
        public virtual DbSet<MixRelatedAttributeSet> MixRelatedAttributeSet { get; set; }
        public virtual DbSet<MixRelatedPost> MixRelatedPost { get; set; }
        public virtual DbSet<MixTemplate> MixTemplate { get; set; }
        public virtual DbSet<MixTheme> MixTheme { get; set; }
        public virtual DbSet<MixUrlAlias> MixUrlAlias { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MixCmsContext(DbContextOptions<MixCmsContext> options)
                    : base(options)
        {
        }

        public MixCmsContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging(true);
            //define the database to use
            string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                var provider = Enum.Parse<MixEnums.DatabaseProvider>(MixService.GetConfig<string>(MixConstants.CONST_SETTING_DATABASE_PROVIDER));
                switch (provider)
                {
                    case MixEnums.DatabaseProvider.MSSQL:
                        optionsBuilder.UseSqlServer(cnn);
                        break;
                    case MixEnums.DatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(cnn);
                        break;
                    case MixEnums.DatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(cnn);
                        break;
                    default:
                        break;
                }
            }
        }

        //Ref https://github.com/dotnet/efcore/issues/10169
        public override void Dispose()
        {
            var provider = Enum.Parse<MixEnums.DatabaseProvider>(MixService.GetConfig<string>(MixConstants.CONST_SETTING_DATABASE_PROVIDER));
            switch (provider)
            {
                case MixEnums.DatabaseProvider.MSSQL:
                    SqlConnection.ClearPool((SqlConnection)Database.GetDbConnection());
                    break;
                case MixEnums.DatabaseProvider.MySQL:
                    MySqlConnection.ClearPool((MySqlConnection)Database.GetDbConnection());
                    break;
                case MixEnums.DatabaseProvider.PostgreSQL:
                    Npgsql.NpgsqlConnection.ClearPool((Npgsql.NpgsqlConnection)Database.GetDbConnection());
                    break;

            }
            base.Dispose();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}