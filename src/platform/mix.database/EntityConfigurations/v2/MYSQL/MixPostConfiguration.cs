using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixPostConfiguration : MySqlEntityBaseConfiguration<MixPost, int>
    {
        public override void Configure(EntityTypeBuilder<MixPost> builder)
        {
            base.Configure(builder);
        }
    }
}
