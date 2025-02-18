using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixMediaConfiguration : EntityBaseConfiguration<MixMedia, Guid>
    {
        public MixMediaConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            builder.ToTable(MixDbDatabaseNames.Media);
            builder.Property(e => e.TenantId)
               .HasColumnName("tenant_id");
            builder.Property(e => e.Description)
               .HasColumnName("description");
             builder.Property(e => e.FileProperties)
               .HasColumnName("file_properties");
            builder.Property(e => e.DisplayName)
               .HasColumnName("display_name");
            builder.Property(e => e.Extension)
               .HasColumnName("extension");
            builder.Property(e => e.FileFolder)
               .HasColumnName("file_folder");
            builder.Property(e => e.FileName)
               .HasColumnName("file_name");
            builder.Property(e => e.FileSize)
               .HasColumnName("file_size");
            builder.Property(e => e.FileType)
               .HasColumnName("file_type");
            builder.Property(e => e.Tags)
               .HasColumnName("tags");
            builder.Property(e => e.Source)
               .HasColumnName("source");
            builder.Property(e => e.TargetUrl)
               .HasColumnName("target_url");
            base.Configure(builder);
        }
    }
}
