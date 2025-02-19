using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb
{
    public class MixDbDbContext : BaseDbContext
    {
        public MixDbDbContext(DatabaseService databaseService)
            : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
            _dbContextType = GetType();
        }

        public DbSet<MixMetadata> MixMetadata { get; set; }
        public DbSet<MixMetadataContentAssociation> MixMetadataContentAssociation { get; set; }
        public DbSet<MixDbEventSubscriber> MixDbEventSubscriber { get; set; }
        public DbSet<MixMenuItem> MixMenuItem { get; set; }
        public DbSet<MixNavigation> MixNavigation { get; set; }
        public DbSet<MixPermission> MixPermission { get; set; }
        public DbSet<MixPermissionEndpoint> MixPermissionEndpoint { get; set; }
        public DbSet<MixUserData> MixUserData { get; set; }
        public DbSet<MixUserPermission> MixUserPermission { get; set; }
        public DbSet<MixDatabaseAssociation> MixDatabaseAssociation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}