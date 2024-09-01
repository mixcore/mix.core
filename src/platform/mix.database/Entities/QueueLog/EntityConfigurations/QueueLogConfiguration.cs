using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.QueueLog;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.QueueLog.EntityConfigurations
{
    internal class QueueLogConfiguration : EntityBaseConfiguration<QueueLog, Guid>
    {
        public QueueLogConfiguration(DatabaseService databaseService): base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<QueueLog> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.TopicId)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.SubscriptionId)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.DataTypeFullName)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Action)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Note)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.State)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixQueueMessageLogState>())
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.StringData)
                .HasColumnType(Config.Text);

            builder.Property(e => e.ObjectData)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnType(Config.Text);

            builder.Property(e => e.Exception)
            .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnType(Config.Text);

            builder.Property(e => e.Subscriptions)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JArray.Parse(v) : new())
             .IsRequired(false)
             .HasColumnType(Config.Text);
        }
    }
}
