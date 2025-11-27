using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Entities
{
    public sealed class WorkflowDbContext : BaseDbContext
    {
        public WorkflowDbContext(DatabaseService databaseService)
            : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
        {
            _dbContextType = GetType();
        }
        public DbSet<Workflow> Workflow { get; set; }
        public DbSet<WorkflowAction> WorkflowAction { get; set; }
        public DbSet<WorkflowTrigger> WorkflowTrigger { get; set; }
        public DbSet<WorkflowActionData> WorkflowActionData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
