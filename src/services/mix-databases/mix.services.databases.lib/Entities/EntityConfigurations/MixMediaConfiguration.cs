using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Services.Databases.Lib.Entities.EntityConfigurations
{
    public class MixMediaConfiguration : EntityBaseConfiguration<MixMedia, int>
    {
        public MixMediaConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            builder.ToTable(MixServicesDatabasesConstants.DatabaseNameMedia);
            base.Configure(builder);
        }
    }
}
