using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixLanguageContentConfiguration : MultilanguageUniqueNameContentBaseConfiguration<MixLanguageContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixLanguageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DefaultContent)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
