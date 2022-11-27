using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base;
using Mix.Database.Services;

namespace mix.services.ecommerce.Domain.Entities.EntityConfigurations
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
        }
    }
}
