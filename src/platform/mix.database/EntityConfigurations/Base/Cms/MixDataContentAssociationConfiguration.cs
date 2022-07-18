using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixDataContentAssociationConfiguration<TConfig> : EntityBaseConfiguration<MixDataContentAssociation, Guid, TConfig>
        where TConfig : IDatabaseConstants
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
