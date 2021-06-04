using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER.Base
{
    public abstract class MultilanguageContentBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where T : MultilanguageContentBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Specificulture)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

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

            builder.Property(e => e.Content)
                .IsRequired()
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.NString}{SqlServerDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqlServerDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }

    }
}
