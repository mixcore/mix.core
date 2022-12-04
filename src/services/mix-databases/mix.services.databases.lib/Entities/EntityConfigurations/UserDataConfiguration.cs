using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Extensions;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Enums;
using System.Linq.Expressions;

namespace Mix.Services.Databases.Lib.Entities.EntityConfigurations
{
    public class UserDataConfiguration : EntityBaseConfiguration<MixUserData, int>
    {
        public UserDataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixUserData> builder)
        {
            builder.ToTable(MixServicesDatabasesConstants.DatabaseNameUserData);
            base.Configure(builder);

            builder.Property(e => e.Avatar)
                .IsRequired(false);

            builder.Property(e => e.ParentType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseParentType>())
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
