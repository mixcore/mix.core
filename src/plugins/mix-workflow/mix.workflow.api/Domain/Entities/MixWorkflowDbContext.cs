using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services;
using Mix.Heart.Enums;

namespace Mix.Workflow.Api.Domain.Entities
{
    public class MixWorkflowDbContext : BaseDbContext
    {
        public MixWorkflowDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
        }

        public MixWorkflowDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
        }
        public DbSet<MixWorkflow> MixWorkflow { get; set; }
    }
}
