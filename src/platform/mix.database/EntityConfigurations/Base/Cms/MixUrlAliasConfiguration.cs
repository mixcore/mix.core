namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixUrlAliasConfiguration<TConfig> : TenantEntityBaseConfiguration<MixUrlAlias, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixUrlAlias> builder)
        {
            base.Configure(builder);
        }
    }
}
