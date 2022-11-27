using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base;
using Mix.Database.Services;

namespace mix.services.ecommerce.Domain.Entities.EntityConfigurations
{
    public class ShoppingCartConfiguration : EntityBaseConfiguration<ShoppingCart, int>
    {
        public ShoppingCartConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.ToTable(EcommerceConstants.DataTableNameShoppingCart);
            base.Configure(builder);
        }
    }
}
