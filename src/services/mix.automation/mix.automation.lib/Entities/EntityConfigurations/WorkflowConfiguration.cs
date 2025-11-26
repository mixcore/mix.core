using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Entities.EntityConfigurations
{
    public class WorkflowConfiguration : SimpleEntityBaseConfiguration<Workflow, int>
    {
        public WorkflowConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<Workflow> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixAutomationConstants.DatabaseNames.Workflow);
            builder.Property(e => e.Title)
            .HasColumnName("title");
            builder.Property(e => e.Description)
               .HasColumnName("description");

            builder.Property(e => e.CreatedDateTime)
                .HasColumnName("created_date_time")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnName("last_modified")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Priority)
                .HasColumnName("priority")
                .HasColumnType(Config.Integer);
        }
    }
}
