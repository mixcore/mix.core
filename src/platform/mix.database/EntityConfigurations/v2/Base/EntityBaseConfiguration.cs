using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using System;

namespace Mix.Database.EntityConfigurations.v2.Base
{
    public abstract class EntityBaseConfiguration<T, TPrimaryKey, TConfig> : IEntityTypeConfiguration<T>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
        where TConfig: IDatabaseConstants
    {
        protected virtual IDatabaseConstants _config { get; set; }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            _config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            string key = $"PK_{typeof(T).Name}";
            builder.HasKey(e => new { e.Id })
                   .HasName(key);

            builder.Property(e => e.Id)
                .HasDefaultValueIf(typeof(TPrimaryKey) == typeof(Guid), "uuid()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CreatedDateTime)
                .HasColumnType(_config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(_config.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnType(_config.Guid);

            builder.Property(e => e.ModifiedBy)
                .HasColumnType(_config.Guid);

            builder.Property(e => e.Priority)
                .HasColumnType(_config.Integer);

            builder.Property(e => e.Priority)
                .HasColumnType(_config.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{_config.String}{_config.SmallLength}")
                .HasCharSet(_config.CharSet);
        }
    }
}
