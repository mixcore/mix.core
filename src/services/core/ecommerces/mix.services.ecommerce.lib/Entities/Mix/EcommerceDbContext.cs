using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;

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

        public DbSet<MixDatabaseAssociation> MixDatabaseAssociation { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderTracking> OrderTracking { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
    }
}
