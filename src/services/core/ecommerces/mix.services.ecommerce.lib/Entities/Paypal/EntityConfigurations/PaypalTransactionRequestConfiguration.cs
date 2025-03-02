using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Payments.Lib.Constants;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.Entities.Paypal.EntityConfigurations
{
    public class PaypalTransactionRequestConfiguration : EntityBaseConfiguration<PaypalTransactionRequest, int>
    {
        public PaypalTransactionRequestConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<PaypalTransactionRequest> builder)
        {
            builder.ToTable(PaypalConstants.DataTableNameRequest);
            base.Configure(builder);
            builder.Property(e => e.ApplicationContext)
            .HasConversion(
                v => v.ToString(Newtonsoft.Json.Formatting.None),
                v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
            .IsRequired(false)
            .HasColumnType(Config.Text);
            builder.Property(e => e.PurchaseUnits)
            .HasConversion(
                v => v.ToString(Newtonsoft.Json.Formatting.None),
                v => !string.IsNullOrEmpty(v) ? JArray.Parse(v) : new())
            .IsRequired(false)
            .HasColumnType(Config.Text);
            builder.Property(e => e.PaymentStatus)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<PaymentStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
