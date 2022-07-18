namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixPageConfiguration<TConfig> : TenantEntityBaseConfiguration<MixPage, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixPage> builder)
        {
            base.Configure(builder);
        }
    }
}
