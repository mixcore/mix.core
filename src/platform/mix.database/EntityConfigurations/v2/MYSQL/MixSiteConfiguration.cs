using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixSiteConfiguration : EntityBaseConfiguration<MixSite, int>
    {
        public override void Configure(EntityTypeBuilder<MixSite> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Description)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
