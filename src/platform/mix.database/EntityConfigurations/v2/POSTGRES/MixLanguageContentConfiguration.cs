using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixLanguageContentConfiguration : PostgresMultilanguageUniqueNameContentBaseConfiguration<MixLanguageContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixLanguageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DefaultContent)
                .IsRequired()
                .HasColumnType($"{_config.NString}{_config.MaxLength}")
                .HasCharSet(_config.CharSet)
                .UseCollation(_config.DatabaseCollation);
        }
    }
}
