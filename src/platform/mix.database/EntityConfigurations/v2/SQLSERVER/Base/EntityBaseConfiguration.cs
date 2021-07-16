using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER.Base
{
    public abstract class EntityBaseConfiguration<T, TPrimaryKey> : IEntityTypeConfiguration<T>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            string key = $"PK_{typeof(T).Name}";
            builder.HasKey(e => new { e.Id })
                   .HasName(key);

            builder.Property(e => e.Id)
               .HasDefaultValueIf(typeof(TPrimaryKey) == typeof(Guid), "NEWID()")
               .ValueGeneratedOnAdd();

            builder.Property(e => e.CreatedDateTime)
                .HasColumnType(SqlServerDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(SqlServerDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnType(SqlServerDatabaseConstants.DataTypes.Guid);

            builder.Property(e => e.ModifiedBy)
                .HasColumnType(SqlServerDatabaseConstants.DataTypes.Guid);

            builder.Property(e => e.Priority)
                .HasColumnType(SqlServerDatabaseConstants.DataTypes.Integer);

            builder.Property(e => e.Priority)
                .HasColumnType(SqlServerDatabaseConstants.DataTypes.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{SqlServerDatabaseConstants.DataTypes.String}{SqlServerDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqlServerDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
