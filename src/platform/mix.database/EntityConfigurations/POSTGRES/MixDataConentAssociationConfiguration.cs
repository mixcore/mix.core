using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixDataConentAssociationConfiguration : PostgresEntityBaseConfiguration<MixDataContentAssociation, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentAssociation> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ParentType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseParentType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
