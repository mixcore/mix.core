using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDatabaseColumnConfiguration : EntityBaseConfiguration<MixDatabaseColumn, int>

    {
        public MixDatabaseColumnConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.MixDatabaseId)
              .HasColumnName("mix_database_id")
              .HasColumnType(Config.Integer);
            builder.Property(e => e.ReferenceId)
              .HasColumnName("reference_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnName("display_name")
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnName("system_name")
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .IsRequired()
                .HasColumnName("mix_database_name")
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DefaultValue)
                .HasColumnName("default_value")
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasColumnName("data_type")
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.Configurations)
                .HasColumnName("configurations")
               .HasConversion(
                   v => v.ToString(Newtonsoft.Json.Formatting.None),
                   v => JObject.Parse(v ?? "{}"))
               .IsRequired(false)
               .HasColumnType($"{Config.String}{Config.MaxLength}");
        }
    }
}
