using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixNavigationConfiguration : EntityBaseConfiguration<MixNavigation, int>
    {
        public MixNavigationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixNavigation> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixDbDatabaseNames.Navigation);
            builder.Property(e => e.Title)
            .HasColumnName("title");
            builder.Property(e => e.Name)
               .HasColumnName("name");
            builder.Property(e => e.Link)
               .HasColumnName("link");
            builder.Property(e => e.TenantId)
               .HasColumnName("tenant_id");
            builder.Property(e => e.Type)
               .HasColumnName("type");
            builder.Property(e => e.Image)
               .HasColumnName("image");
        }
    }
}