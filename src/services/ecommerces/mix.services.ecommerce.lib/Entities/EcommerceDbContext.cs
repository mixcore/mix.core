using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services;
using Mix.Heart.Enums;

namespace Mix.Services.Ecommerce.Lib.Entities
{
    public class EcommerceDbContext : BaseDbContext
    {
        public EcommerceDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
        }

        public EcommerceDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
        }

        public DbSet<ProductDetails> ProductDetails { get; set; }
    }
}
