using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixMediaConfiguration : EntityBaseConfiguration<MixDbMedia, Guid>
    {
        public MixMediaConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixDbMedia> builder)
        {
            builder.ToTable(MixDbDatabaseNames.DatabaseNameMedia);
            base.Configure(builder);
        }
    }
}
