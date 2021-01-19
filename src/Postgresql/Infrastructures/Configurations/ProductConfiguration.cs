using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Postgresql.Infrastructures.Domain;

namespace Postgresql.Infrastructures.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.Category)
                .WithMany(p => p.Products);

            builder.Property(p => p.IsPublic)
                .HasColumnType("boolean")
                .UseCollation("npgsql_default_collation")
                .IsUnicode();
        }
    }
}
