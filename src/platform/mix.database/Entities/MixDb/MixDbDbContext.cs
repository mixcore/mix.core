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

        public DbSet<MixDbEventSubscriber> MixDbEventSubscriber { get; set; }
        public DbSet<MixDbMedia> MixMedia { get; set; }
        public DbSet<MixMetadata> MixMetadata { get; set; }
        public DbSet<MixMetadataContentAssociation> MixMetadataContentAssociation { get; set; }
        public DbSet<MixDatabaseAssociation> MixDatabaseAssociation { get; set; }
        public DbSet<MixPermission> Permission { get; set; }
        public DbSet<MixPermissionEndpoint> PermissionEndpoint { get; set; }
        public DbSet<MixUserPermission> UserPermission { get; set; }
        public DbSet<MixUserData> MixUserData { get; set; }
        public DbSet<MixNavigation> MixNavigation { get; set; }
        public DbSet<MixMenuItem> MixMenuItem { get; set; }
    }
}