using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using System;
using Mix.Database.EntityConfigurations.v2;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES.Base
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
                .HasDefaultValueIf(typeof(TPrimaryKey) == typeof(Guid), "uuid_generate_v4()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CreatedDateTime)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.Guid);

            builder.Property(e => e.ModifiedBy)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.Guid);

            builder.Property(e => e.Priority)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.Integer);

            builder.Property(e => e.Priority)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.String}{PostgresSqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet);
        }
    }
}
