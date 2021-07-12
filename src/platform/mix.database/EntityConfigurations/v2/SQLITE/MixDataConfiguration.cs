using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixDataConfiguration : EntityBaseConfiguration<MixData, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
