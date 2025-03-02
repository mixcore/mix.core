using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;

namespace Mix.Services.Ecommerce.Lib.Entities.Onepay
{
    public class OnepayDbContext : BaseDbContext
    {
        public OnepayDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
        }

        public OnepayDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
        }
        public DbSet<OnepayTransactionRequest> OnepayTransactionRequest { get; set; }
        public DbSet<OnepayTransactionResponse> OnepayTransactionResponse { get; set; }
    }
}
