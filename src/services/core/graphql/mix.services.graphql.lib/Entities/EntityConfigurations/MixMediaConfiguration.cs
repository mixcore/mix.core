using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;


namespace Mix.Services.Graphql.Lib.Entities.EntityConfigurations
{
    public class MixMediaConfiguration : EntityBaseConfiguration<MixMedia, int>
    {
        public MixMediaConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            builder.ToTable(MixDbDatabaseNames.Media);
            base.Configure(builder);
        }
    }
}
