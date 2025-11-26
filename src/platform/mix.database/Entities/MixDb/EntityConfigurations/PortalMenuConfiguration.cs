using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class PortalMenuConfiguration : EntityBaseConfiguration<MixPortalMenu, int>
    {
        public PortalMenuConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixPortalMenu> builder)
        {
            builder.ToTable(MixDbDatabaseNames.PortalMenu);
            base.Configure(builder);

            builder.Property(e => e.Title)
                .IsRequired(false)
                .HasColumnName("title");
            builder.Property(e => e.Svg)
                .IsRequired(false)
                .HasColumnName("svg");
            builder.Property(e => e.Icon)
                .IsRequired(false)
                .HasColumnName("icon");
            builder.Property(e => e.Path)
                .IsRequired(false)
                .HasColumnName("path");
            builder.Property(e => e.Role)
                .IsRequired(false)
                .HasColumnName("role");
        }
    }
}
