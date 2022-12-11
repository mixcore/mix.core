using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base;
using Mix.Database.Services;
using Mix.Services.Payments.Lib.Constants;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix.EntityConfigurations
{
    public class OrderItemConfiguration : EntityBaseConfiguration<OrderItem, int>
    {
        public OrderItemConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable(EcommerceConstants.DataTableNameOrderItem);
            base.Configure(builder);
            builder.Property(e => e.Currency).IsRequired(false);
        }
    }
}
