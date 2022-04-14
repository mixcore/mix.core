using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.SQLITE.Base;


namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class MixDataConentAssociationConfiguration : SqliteEntityBaseConfiguration<MixDataContentAssociation, Guid>
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
