using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Services.Ecommerce.Lib.Constants;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix.EntityConfigurations
{
    public class WarehouseConfiguration : EntityBaseConfiguration<Warehouse, int>
    {
        public WarehouseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable(MixEcommerceConstants.DatabaseNameWarehosue);
            base.Configure(builder);

            builder.Property(e => e.Sku).IsRequired();
        }
    }
}
