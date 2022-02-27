using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixModuleDataConfiguration : PostgresMultilanguageSEOContentBaseConfiguration<MixModuleData, int>
    {
        public override void Configure(EntityTypeBuilder<MixModuleData> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SimpleDataColumns)
               .HasColumnType($"{Config.String}{Config.MaxLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.Value)
               .HasColumnType($"{Config.String}{Config.MaxLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
