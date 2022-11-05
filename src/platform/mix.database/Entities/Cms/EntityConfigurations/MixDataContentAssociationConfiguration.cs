using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDataContentAssociationConfiguration : EntityBaseConfiguration<MixDataContentAssociation, Guid>
        
    {
        public MixDataContentAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

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
