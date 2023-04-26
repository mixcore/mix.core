
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPageContentConfiguration : MultilingualSEOContentBaseConfiguration<MixPageContent, int>
    {
        public MixPageContentConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixPageType>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
