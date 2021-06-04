using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixDataConfiguration : EntityBaseConfiguration<MixData, int>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
              .HasCharSet(DatabaseConfiguration.CharSet)
              .UseCollation(DatabaseConfiguration.DatabaseCollation);
        }
    }
}
