using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Mixdb.Entities
{
    public class MixDbDbContext : BaseDbContext
    {
        public MixDbDbContext(DatabaseService databaseService)
            : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
            _dbContextType = GetType();
        }

        public DbSet<MixMedia> MixMedia { get; set; }
        public DbSet<MixMetadata> MixMetadata { get; set; }
        public DbSet<MixMetadataContentAssociation> MixMetadataContentAssociation { get; set; }
        public DbSet<MixPermission> Permission { get; set; }
        public DbSet<MixPermissionEndpoint> PermissionEndpoint { get; set; }
        public DbSet<MixUserPermission> UserPermission { get; set; }
        public DbSet<MixUserData> MixUserData { get; set; }
    }
}
