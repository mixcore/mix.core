using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixSiteConfiguration : EntityBaseConfiguration<MixSite, int>
    {
        public override void Configure(EntityTypeBuilder<MixSite> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
