using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services;

namespace Mix.Mixdb.Entities
{
    public class MixDbDbContext : BaseDbContext
    {
        public MixDbDbContext(DatabaseService databaseService)
            : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
            _dbContextType = GetType();
        }

        public DbSet<MixDbEventSubscriber> MixDbEventSubscriber { get; set; }
        public DbSet<MixMedia> MixMedia { get; set; }
        public DbSet<MixMetadata> MixMetadata { get; set; }
        public DbSet<MixMetadataContentAssociation> MixMetadataContentAssociation { get; set; }
        public DbSet<MixPermission> Permission { get; set; }
        public DbSet<MixPermissionEndpoint> PermissionEndpoint { get; set; }
        public DbSet<MixUserPermission> UserPermission { get; set; }
        public DbSet<MixUserData> MixUserData { get; set; }
        public DbSet<MixContactAddress> MixContactAddress { get; set; }
    }
}
