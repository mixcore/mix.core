using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Heart.Entity;
using Mix.Heart.Enums;

namespace Mix.Database.EntityConfigurations.v2.MYSQL.Base
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
                .HasColumnType(MySqlDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(MySqlDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Guid);

            builder.Property(e => e.ModifiedBy)
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Guid);

            builder.Property(e => e.Priority)
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Integer);

            builder.Property(e => e.Priority)
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.String}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
