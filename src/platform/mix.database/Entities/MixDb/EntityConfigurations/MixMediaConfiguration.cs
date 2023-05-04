using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixMediaConfiguration : EntityBaseConfiguration<MixMedia, int>
    {
        public MixMediaConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            builder.ToTable(MixDbDatabaseNames.DatabaseNameMedia);
            base.Configure(builder);
        }
    }
}
