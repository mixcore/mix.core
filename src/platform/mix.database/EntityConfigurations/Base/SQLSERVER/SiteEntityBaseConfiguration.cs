using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class SiteEntityBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where T : SiteEntityBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Image)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8");
            
            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8");
            
            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8");
            
            builder.Property(e => e.Description)
                .IsRequired()
                .HasColumnType("varchar(4000)")
                .HasCharSet("utf8");
        }

    }
}
