using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Services.Ecommerce.Lib.Enums;
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
            builder.Property(e => e.ItemType)
                .IsRequired(false)
                .HasConversion(new EnumToStringConverter<OrderItemType>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.Currency).IsRequired(false);
            builder.Property(e => e.Title).IsRequired(false);
            builder.Property(e => e.Description).IsRequired(false);
            builder.Property(e => e.Image).IsRequired(false);
            builder.Property(e => e.ReferenceUrl).IsRequired(false);
        }
    }
}
