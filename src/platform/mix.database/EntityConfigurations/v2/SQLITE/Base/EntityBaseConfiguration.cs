using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.SQLITE.Base
{
    public abstract class EntityBaseConfiguration<T, TPrimaryKey> : IEntityTypeConfiguration<T>
        where T : EntityBase<TPrimaryKey>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            string key = $"PK_{typeof(T).Name}";
            builder.HasKey(e => new { e.Id })
                   .HasName(key);

            builder.Property(e => e.CreatedDateTime)
                .HasColumnType(SqliteDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(SqliteDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Guid);

            builder.Property(e => e.ModifiedBy)
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Guid);

            builder.Property(e => e.Priority)
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Integer);

            builder.Property(e => e.Priority)
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.String}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
