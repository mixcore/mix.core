using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Extensions;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Entities.EntityConfigurations
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
            builder.ConfigueJsonColumn(p => p.Metadata, _databaseService);
            builder.Property(e => e.ParentType)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<MixDatabaseParentType>())
            .HasColumnType($"{Config.String}{Config.SmallLength}")
            .HasCharSet(Config.CharSet);
        }
    }
}
