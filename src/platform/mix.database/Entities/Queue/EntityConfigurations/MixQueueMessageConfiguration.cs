using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.Queue.EntityConfigurations
{
    internal class MixQueueMessageConfiguration : IEntityTypeConfiguration<MixQueueMessage>
    {
        public IDatabaseConstants Config;
        public MixQueueMessageConfiguration()
        {
            Config = new SqliteDatabaseConstants();
        }
        public void Configure(EntityTypeBuilder<MixQueueMessage> builder)
        {
            builder.Property(e => e.Action)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            
            builder.Property(e => e.From)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            
            builder.Property(e => e.State)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.StringData)
                .HasColumnType(Config.Text);

            builder.Property(e => e.IsDeleted)
               .HasColumnType(Config.Boolean);

            builder.Property(e => e.CreatedDateTime)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ModifiedBy)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Priority)
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.ObjectData)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnType(Config.Text);
        }
    }
}
