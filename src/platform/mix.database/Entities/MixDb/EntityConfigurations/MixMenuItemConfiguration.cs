using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixMenuItemConfiguration : EntityBaseConfiguration<MixMenuItem, int>
    {
        public MixMenuItemConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMenuItem> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixDbDatabaseNames.MenuItem);
            builder.Property(e => e.Title)
            .HasColumnName("title");
            builder.Property(e => e.Target)
               .HasColumnName("target");
            builder.Property(e => e.Classes)
               .HasColumnName("classes");
            builder.Property(e => e.Alt)
               .HasColumnName("alt");
            builder.Property(e => e.Description)
               .HasColumnName("description");
            builder.Property(e => e.Group)
               .HasColumnName("group");
            builder.Property(e => e.Hreflang)
               .HasColumnName("href_lang");
            builder.Property(e => e.Icon)
               .HasColumnName("icon");
            builder.Property(e => e.Image)
               .HasColumnName("image");
            builder.Property(e => e.TargetId)
               .HasColumnName("target_id");
            builder.Property(e => e.TenantId)
               .HasColumnName("tenant_id");
        }
    }
}