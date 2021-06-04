using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixPostConfiguration : EntityBaseConfiguration<MixPost, int>
    {
        public override void Configure(EntityTypeBuilder<MixPost> builder)
        {
            base.Configure(builder);
        }
    }
}
