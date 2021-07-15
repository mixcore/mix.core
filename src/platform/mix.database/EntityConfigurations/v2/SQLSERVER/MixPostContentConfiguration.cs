using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixPostContentConfiguration : MultilanguageSEOContentBaseConfiguration<MixPostContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixPostContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.String}{SqlServerDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
