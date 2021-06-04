using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLITE.Base
{
    public abstract class MultilanguageContentBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where T : MultilanguageContentBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Specificulture)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Content)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }

    }
}
