namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixLanguageConfiguration<TConfig> : TenantEntityUniqueNameBaseConfiguration<MixLanguage, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixLanguage> builder)
        {
            base.Configure(builder);
        }
    }
}
