using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixNavigationConfiguration : EntityBaseConfiguration<MixNavigation, int>
    {
        public MixNavigationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixNavigation> builder)
        {
            builder.ToTable(MixDbDatabaseNames.Navigation);
            base.Configure(builder);
        }
    }
}