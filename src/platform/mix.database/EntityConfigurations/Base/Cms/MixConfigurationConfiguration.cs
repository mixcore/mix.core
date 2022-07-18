namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixConfigurationConfiguration<TConfig> : TenantEntityUniqueNameBaseConfiguration<MixConfiguration, int, TConfig>
         where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixConfiguration> builder)
        {
            base.Configure(builder);
        }
    }
}
