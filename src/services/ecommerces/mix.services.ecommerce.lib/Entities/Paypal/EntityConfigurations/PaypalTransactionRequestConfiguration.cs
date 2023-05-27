using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Payments.Lib.Constants;

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
            builder.Property(e => e.vpc_Currency)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Command)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_AccessCode)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Merchant)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Locale)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_ReturnURL)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MaxLength}");
            builder.Property(e => e.vpc_MerchTxnRef)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_OrderInfo)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Amount)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_TicketNo)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.AgainLink)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.Title)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_SecureHash)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Customer_Phone)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Customer_Email)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Customer_Id)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.PaypalStatus)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<OrderStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
