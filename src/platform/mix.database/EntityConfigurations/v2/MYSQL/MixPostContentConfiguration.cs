using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixPostContentConfiguration : MultilanguageSEOContentBaseConfiguration<MixPostContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixPostContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.String}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
