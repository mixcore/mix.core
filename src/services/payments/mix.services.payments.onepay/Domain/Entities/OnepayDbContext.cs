using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services;
using Mix.Heart.Enums;

namespace Mix.Services.Payments.Onepay.Domain.Entities
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
