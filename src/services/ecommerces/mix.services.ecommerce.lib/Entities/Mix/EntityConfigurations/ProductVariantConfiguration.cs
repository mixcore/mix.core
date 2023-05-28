using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Services.Ecommerce.Lib.Constants;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix.EntityConfigurations
{
    public class ProductVariantConfiguration : EntityBaseConfiguration<Warehouse, int>
    {
        public ProductVariantConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable(MixEcommerceConstants.DatabaseNameProductVariant);
            base.Configure(builder);

            builder.Property(e => e.Sku).IsRequired();
        }
    }
}
