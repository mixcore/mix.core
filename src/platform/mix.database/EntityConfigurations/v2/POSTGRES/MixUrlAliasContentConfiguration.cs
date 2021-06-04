using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixUrlAliasContentConfiguration : EntityBaseConfiguration<MixUrlAliasContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAliasContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SourceId)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Alias)
                .IsRequired()
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<MixUrlAliasType>())
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
