using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Extensions;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Enums;
using System.Linq.Expressions;

namespace Mix.Services.Databases.Lib.Entities.EntityConfigurations
{
    public class ContactAddressConfiguration : EntityBaseConfiguration<MixContactAddress, int>
    {
        public ContactAddressConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixContactAddress> builder)
        {
            builder.ToTable(MixServicesDatabasesConstants.DatabaseNameContactAddress);
            base.Configure(builder);

            builder.Property(e => e.Street)
                .IsRequired(false);
            builder.Property(e => e.City)
                .IsRequired(false);
            builder.Property(e => e.Province)
                .IsRequired(false);
            builder.Property(e => e.District)
                .IsRequired(false);
            builder.Property(e => e.Note)
                .IsRequired(false);
        }
    }
}
