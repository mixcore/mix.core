using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Automation.Lib.Enums;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Entities.EntityConfigurations
{
    public class WorkflowActionDataConfiguration : SimpleEntityBaseConfiguration<WorkflowActionData, int>
    {
        public WorkflowActionDataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<WorkflowActionData> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixAutomationConstants.DatabaseNames.WorkflowActionData);

            builder.Property(e => e.Index)
                .HasColumnName("index");

            builder.Property(e => e.ActionType)
                .HasColumnName("action_type")
                .HasConversion(new EnumToStringConverter<ActionType>());

            builder.Property(e => e.ActionStatus)
                .HasColumnName("action_status")
                .HasConversion(new EnumToStringConverter<ActionStatus>());

            builder.Property(e => e.IsSuccess)
            .HasColumnName("is_success");

            builder.Property(e => e.Body)
            .HasConversion(
                v => v.ToString(Newtonsoft.Json.Formatting.None),
                v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : default)
            .IsRequired(false)
            .HasColumnName("body")
            .HasColumnType(Config.Text);

            builder.Property(e => e.Request)
             .HasConversion(
                 v => v != default ? v.ToString(Newtonsoft.Json.Formatting.None) : default,
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : default)
             .IsRequired(false)
             .HasColumnName("request")
             .HasColumnType(Config.Text);

            builder.Property(e => e.Response)
             .HasConversion(
                 v => v != default ? v.ToString(Newtonsoft.Json.Formatting.None) : default,
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : default)
             .IsRequired(false)
             .HasColumnName("response")
             .HasColumnType(Config.Text);

            builder.Property(e => e.Exception)
             .HasConversion(
                 v => v != default ? v.ToString(Newtonsoft.Json.Formatting.None) : default,
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : default)
             .IsRequired(false)
             .HasColumnName("exception")
             .HasColumnType(Config.Text);

            builder.Property(e => e.Duration)
            .HasColumnName("duration");
            
            builder.Property(e => e.TriggerId)
            .HasColumnName("mix_workflow_trigger_id");
            
            builder.Property(e => e.ActionId)
            .HasColumnName("mix_workflow_action_id");
            
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
