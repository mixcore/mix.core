using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
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
                .HasColumnType("datetime");
            
            builder.Property(e => e.LastModified)
                .HasColumnType("datetime");

            builder.Property(e => e.CreatedBy)
                .HasColumnType("uniqueidentifier");
            
            builder.Property(e => e.ModifiedBy)
                .HasColumnType("uniqueidentifier");
            
            builder.Property(e => e.Priority)
                .HasColumnType("int");
            
            builder.Property(e => e.Priority)
                .HasColumnType("int");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8");
        }
    }
}
