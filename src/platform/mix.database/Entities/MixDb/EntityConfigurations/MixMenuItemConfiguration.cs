using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixMenuItemConfiguration : EntityBaseConfiguration<MixMenuItem, int>
    {
        public MixMenuItemConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMenuItem> builder)
        {
            builder.ToTable(MixDbDatabaseNames.MenuItem);
            base.Configure(builder);
        }
    }
}