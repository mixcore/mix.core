using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Newtonsoft.Json.Linq;
using RepoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Entities.EntityConfigurations
{
    public class WorkflowTriggerConfiguration : SimpleEntityBaseConfiguration<WorkflowTrigger, int>
    {
        public WorkflowTriggerConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<WorkflowTrigger> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixAutomationConstants.DatabaseNames.WorkflowTrigger);

            builder.Property(e => e.IsSuccess)
            .HasColumnName("is_success");

            builder.Property(e => e.Input)
             .HasConversion(
                 v => v != default ? v.ToString(Newtonsoft.Json.Formatting.None) : default,
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : default)
             .IsRequired(false)
             .HasColumnName("input")
             .HasColumnType(Config.Text);

            builder.Property(e => e.WorkflowId)
               .HasColumnName("mix_workflow_id");

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
