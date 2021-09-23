using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.MYSQL.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MixDomainConfiguration : MySqlTenantEntityBaseConfiguration<MixDomain, int>
    {
        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
        }
    }
}
