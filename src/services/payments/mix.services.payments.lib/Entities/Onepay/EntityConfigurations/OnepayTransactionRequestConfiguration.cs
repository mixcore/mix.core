using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base;
using Mix.Database.Services;
using Mix.Services.Payments.Lib.Constants;
using Mix.Services.Payments.Lib.Enums;

namespace Mix.Services.Payments.Lib.Entities.Onepay.EntityConfigurations
{
    public class OnepayTransactionRequestConfiguration : EntityBaseConfiguration<OnepayTransactionRequest, int>
    {
        public OnepayTransactionRequestConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<OnepayTransactionRequest> builder)
        {
            builder.ToTable(OnepayConstants.DataTableNameRequest);
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
            builder.Property(e => e.OnepayStatus)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<PaymentStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
