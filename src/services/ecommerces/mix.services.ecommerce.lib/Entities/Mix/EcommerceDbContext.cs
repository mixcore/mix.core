using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using RepoDb.Enumerations;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix
{
    public class EcommerceDbContext : BaseDbContext
    {
        public EcommerceDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
        }

        public EcommerceDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
        }

        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderTracking> OrderTracking { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<ProductDetails> ProductDetails { get; set; }
        public DbSet<ProductVariant> ProductVariant { get; set; }
        public DbSet<MixDatabaseAssociation> MixDatabaseAssociation { get; set; }
    }
}
