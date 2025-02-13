using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Extensions;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixContributorConfiguration : EntityBaseConfiguration<MixContributor, int>

    {
        public MixContributorConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixContributor> builder)
        {
            base.Configure(builder);
            
            builder.Property(e => e.IsOwner)
              .HasColumnName("is_owner")
              .HasColumnType(Config.Boolean);

            builder.Property(e => e.IntContentId)
              .HasColumnName("int_content_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.GuidContentId)
              .HasColumnName("guid_content_id")
              .HasColumnType(Config.Guid);

            builder.Property(e => e.UserId)
              .HasColumnName("use_id")
              .HasColumnType(Config.Guid);

            builder.Property(e => e.TenantId)
              .HasColumnName("tenant_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.UserId)
              .HasColumnName("use_id");

            builder.Property(e => e.ContentType)
              .IsRequired()
              .HasColumnName("content_type")
              .HasConversion(new EnumToStringConverter<MixContentType>())
              .HasColumnType($"{Config.NString}{Config.SmallLength}")
              .HasCharSet(Config.CharSet);
        }
    }
}
