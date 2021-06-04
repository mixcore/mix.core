using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Base
{
    public abstract class MultilanguageSEOContentBaseConfiguration<T, TPrimaryKey> : MultilanguageContentBaseConfiguration<T, TPrimaryKey>
        where T : MultilanguageSEOContentBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8");

            builder.Property(e => e.PublishedDateTime)
                .HasColumnType("datetime");
            
            builder.Property(e => e.Image)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8");

            builder.Property(e => e.Source)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8");

            builder.Property(e => e.SeoDescription)
                .HasColumnType("varchar(4000)")
                .HasCharSet("utf8");
            
            builder.Property(e => e.SeoKeywords)
                .HasColumnType("varchar(4000)")
                .HasCharSet("utf8");
            
            builder.Property(e => e.SeoName)
                .HasColumnType("varchar(250)")
                .HasCharSet("utf8");
            
            builder.Property(e => e.SeoName)
                .HasColumnType("varchar(4000)")
                .HasCharSet("utf8");
            
        }

    }
}
