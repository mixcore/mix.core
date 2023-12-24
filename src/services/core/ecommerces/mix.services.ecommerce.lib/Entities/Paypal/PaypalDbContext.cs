using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services;
using Mix.Heart.Enums;

namespace Mix.Services.Ecommerce.Lib.Entities.Paypal
{
    public class PaypalDbContext : BaseDbContext
    {
        public PaypalDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
        }

        public PaypalDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
        }
        public DbSet<PaypalTransactionRequest> PaypalTransactionRequest { get; set; }
        public DbSet<PaypalTransactionResponse> PaypalTransactionResponse { get; set; }
    }
}
