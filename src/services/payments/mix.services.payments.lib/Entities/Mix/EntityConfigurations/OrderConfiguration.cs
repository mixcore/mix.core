﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base;
using Mix.Database.Services;
using Mix.Services.Payments.Lib.Constants;
using Mix.Services.Payments.Lib.Enums;

namespace Mix.Services.Payments.Lib.Entities.Mix.EntityConfigurations
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
            builder.Property(e => e.Currency).IsRequired(false);
            builder.Property(e => e.Description).IsRequired(false);

            builder.Property(e => e.PaymentGateway)
                .IsRequired(false)
                .HasConversion(new EnumToStringConverter<PaymentGateway>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.OrderStatus)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<OrderStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}