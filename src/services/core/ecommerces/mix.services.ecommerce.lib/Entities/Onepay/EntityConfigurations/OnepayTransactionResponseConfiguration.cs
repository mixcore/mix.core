using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Payments.Lib.Constants;

namespace Mix.Services.Ecommerce.Lib.Entities.Onepay.EntityConfigurations
{
    public class OnepayTransactionResponseConfiguration : EntityBaseConfiguration<OnepayTransactionResponse, int>
    {
        public OnepayTransactionResponseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<OnepayTransactionResponse> builder)
        {
            builder.ToTable(OnepayConstants.DataTableNameResponse);
            base.Configure(builder);
            builder.Property(e => e.vpc_Command)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Locale)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_CurrencyCode)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_MerchTxnRef)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Merchant)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_OrderInfo)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Amount)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_TxnResponseCode)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_TransactionNo)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_Message)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_AdditionData)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.vpc_SecureHash)
                .IsRequired(false)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            builder.Property(e => e.OnepayStatus)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<OrderStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
