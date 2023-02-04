using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Mixdb.Entities.EntityConfigurations
{
    public class PortalMenuConfiguration : EntityBaseConfiguration<MixPortalMenu, int>
    {
        public PortalMenuConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixPortalMenu> builder)
        {
            builder.ToTable(MixDbDatabaseNames.DatabaseNamePortalMenu);
            base.Configure(builder);

            builder.Property(e => e.Title)
                .IsRequired(false);
            builder.Property(e => e.Svg).IsRequired(false);
            builder.Property(e => e.Icon)
                .IsRequired(false);
            builder.Property(e => e.Path)
                .IsRequired(false);
            builder.Property(e => e.Role)
                .IsRequired(false);
        }
    }
}
