using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixDataConfiguration : EntityBaseConfiguration<MixData, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
