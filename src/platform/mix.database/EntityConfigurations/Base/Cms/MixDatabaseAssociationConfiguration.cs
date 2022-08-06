namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixDatabaseAssociationConfiguration<TConfig> : EntityBaseConfiguration<MixDatabaseAssociation, Guid, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseAssociation> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ParentDatabaseName)
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.ChildDatabaseName)
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
