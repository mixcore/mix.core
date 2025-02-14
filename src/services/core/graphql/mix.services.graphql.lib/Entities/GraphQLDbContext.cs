using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Services.Graphql.Lib.Entities
{
    public class GraphQLDbContext : BaseDbContext
    {
        public GraphQLDbContext(DatabaseService databaseService)
            : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
            _dbContextType = GetType();
        }

        public DbSet<MixMedia> MixMedia { get; set; }
        public DbSet<MixMetadata> MixMetadata { get; set; }
    }
}
