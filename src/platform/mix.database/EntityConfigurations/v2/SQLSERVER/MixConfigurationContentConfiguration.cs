using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixConfigurationContentConfiguration : MultilanguageUniqueNameContentBaseConfiguration<MixConfigurationContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixConfigurationContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DefaultContent)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
