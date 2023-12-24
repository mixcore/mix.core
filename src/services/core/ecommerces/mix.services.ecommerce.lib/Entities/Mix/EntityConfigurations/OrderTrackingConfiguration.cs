using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Payments.Lib.Constants;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix.EntityConfigurations
{
    public class OrderTrackingConfiguration : EntityBaseConfiguration<OrderTracking, int>
    {
        public OrderTrackingConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<OrderTracking> builder)
        {
            builder.ToTable(EcommerceConstants.DataTableNameOrderTracking);
            base.Configure(builder);
            builder.Property(e => e.Note).IsRequired(false);
            builder.Property(e => e.Action)
                .IsRequired(false)
                .HasConversion(new EnumToStringConverter<OrderTrackingAction>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
