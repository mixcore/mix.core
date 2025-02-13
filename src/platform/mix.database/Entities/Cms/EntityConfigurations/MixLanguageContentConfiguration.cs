using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

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
            builder.Property(e => e.MixLanguageId)
              .HasColumnName("mix_language_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.DefaultContent)
                .IsRequired(false)
                .HasColumnName("default_content")
                .HasColumnType($"{Config.NString}{Config.MaxLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
            
            builder.Property(e => e.Category)
                .IsRequired(false)
                .HasColumnName("category")
                .HasColumnType($"{Config.NString}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.DataType)
            .IsRequired()
            .HasColumnName("data_type")
            .HasConversion(new EnumToStringConverter<MixDataType>())
            .HasColumnType($"{Config.String}{Config.SmallLength}")
            .HasCharSet(Config.CharSet)
            .UseCollation(Config.DatabaseCollation);
        }
    }
}
