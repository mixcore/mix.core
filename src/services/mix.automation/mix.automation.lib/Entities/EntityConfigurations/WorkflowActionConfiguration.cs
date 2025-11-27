using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Automation.Lib.Enums;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Entities.EntityConfigurations
{
    public class WorkflowActionConfiguration : SimpleEntityBaseConfiguration<WorkflowAction, int>
    {
        public WorkflowActionConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<WorkflowAction> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixAutomationConstants.DatabaseNames.WorkflowAction);

            builder.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion(new EnumToStringConverter<ActionType>());

            builder.Property(e => e.Body)
            .HasConversion(
                v => v != default ? v.ToString(Newtonsoft.Json.Formatting.None) : default,
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

            builder.Property(e => e.Type)
            .HasColumnName("type");

            builder.Property(e => e.Index)
            .HasColumnName("index");
            
            builder.Property(e => e.WorkflowId)
            .HasColumnName("mix_workflow_id");
            
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
