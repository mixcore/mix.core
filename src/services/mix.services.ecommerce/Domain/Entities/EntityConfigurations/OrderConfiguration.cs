using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mix.services.ecommerce.Domain.Enums;
using Mix.Database.Base;
using Mix.Database.Services;
using Mix.Heart.Enums;

namespace mix.services.ecommerce.Domain.Entities.EntityConfigurations
{
    public class OrderConfiguration : EntityBaseConfiguration<Order, int>
    {
        public OrderConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(EcommerceConstants.DataTableNameOrder);
            base.Configure(builder);
            builder.Property(e => e.OrderStatus)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<OrderStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
