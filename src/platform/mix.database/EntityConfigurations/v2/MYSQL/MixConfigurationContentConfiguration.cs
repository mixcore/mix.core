using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixConfigurationContentConfiguration : MySqlMultilanguageUniqueNameContentBaseConfiguration<MixConfigurationContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixConfigurationContent> builder)
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
