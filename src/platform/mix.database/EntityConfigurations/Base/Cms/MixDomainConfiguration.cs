namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixDomainConfiguration<TConfig> : TenantEntityBaseConfiguration<MixDomain, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Host)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
