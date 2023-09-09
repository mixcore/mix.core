using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Workflow.Api.Domain.Constants;

namespace Mix.Workflow.Api.Domain.Entities.EntityConfigurations
{
    public class MixWorkflowConfiguration : EntityBaseConfiguration<MixWorkflow, int>
    {
        public MixWorkflowConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixWorkflow> builder)
        {
            builder.ToTable(MixWorkflowConstants.WorkflowDatabaseName);
        }
    }
}
