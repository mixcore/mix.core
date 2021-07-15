using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES.Base
{
    public abstract class SiteEntityUniqueNameBaseConfiguration<T, TPrimaryKey> : SiteEntityBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : SiteEntityUniqueNameBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SystemName)
               .IsRequired()
               .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
               .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

        }

    }
}
