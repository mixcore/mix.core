using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MetadataContentAssociationConfiguration : EntityBaseConfiguration<MixMetadataContentAssociation, int>
    {
        public MetadataContentAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMetadataContentAssociation> builder)
        {
            builder.ToTable(MixDbDatabaseNames.MetadataContentAssociation);
            base.Configure(builder);

            builder.Property(e => e.TenantId)
                .HasColumnName("tenant_id");
            builder.Property(e => e.ContentType)
               .IsRequired(false)
               .HasColumnName("content_type")
               .HasConversion(new EnumToStringConverter<MixContentType>())
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
