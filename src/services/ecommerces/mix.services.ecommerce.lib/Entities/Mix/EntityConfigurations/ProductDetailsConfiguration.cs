﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Extensions;
using Mix.Database.Services;
using Mix.Services.Ecommerce.Lib.Constants;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix.EntityConfigurations
{
    public class ProductDetailsConfiguration : EntityBaseConfiguration<ProductDetails, int>
    {
        public ProductDetailsConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<ProductDetails> builder)
        {
            builder.ToTable(MixEcommerceConstants.DatabaseNameProductDetails);
            base.Configure(builder);

            builder.Property(e => e.DesignBy).IsRequired(false);
            builder.Property(e => e.Information).IsRequired(false);
            builder.Property(e => e.InformationImage).IsRequired(false);
            builder.Property(e => e.Size).IsRequired(false);
            builder.Property(e => e.SizeImage).IsRequired(false);
            builder.Property(e => e.Document).IsRequired(false);
            builder.Property(e => e.MaintenanceDocument).IsRequired(false);

            builder.Property(e => e.ParentType)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<MixDatabaseParentType>())
            .HasColumnType($"{Config.String}{Config.SmallLength}")
            .HasCharSet(Config.CharSet);
        }
    }
}