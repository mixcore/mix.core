using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixSiteConfiguration : EntityBaseConfiguration<MixSite, int>
    {
        public override void Configure(EntityTypeBuilder<MixSite> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Description)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
