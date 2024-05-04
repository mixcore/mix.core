using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixLanguageContentConfiguration : MultilingualUniqueNameContentBaseConfiguration<MixLanguageContent, int>

    {
        public MixLanguageContentConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixLanguageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DefaultContent)
                .IsRequired(false)
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Category)
                .IsRequired(false)
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DataType)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<MixDataType>())
            .HasColumnType($"{Config.String}{Config.SmallLength}")
            .HasCharSet(Config.CharSet)
            .UseCollation(Config.DatabaseCollation);
        }
    }
}
