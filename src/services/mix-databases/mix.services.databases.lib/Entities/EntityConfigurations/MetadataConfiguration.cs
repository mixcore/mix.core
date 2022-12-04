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
    public class MixMetadataConfiguration : EntityBaseConfiguration<MixMetadata, int>
    {
        public MixMetadataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMetadata> builder)
        {
            builder.ToTable(MixServicesDatabasesConstants.DatabaseNameMetadata);
            base.Configure(builder);
        }
    }
}
