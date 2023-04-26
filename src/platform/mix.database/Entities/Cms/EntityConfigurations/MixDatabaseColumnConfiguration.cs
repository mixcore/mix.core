using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
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

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .IsRequired()
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DefaultValue)
                .HasColumnType(Config.Text)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DataType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDataType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.Configurations)
               .HasConversion(
                   v => v.ToString(Newtonsoft.Json.Formatting.None),
                   v => JObject.Parse(v ?? "{}"))
               .IsRequired(false)
               .HasColumnType($"{Config.String}{Config.MaxLength}");
        }
    }
}
