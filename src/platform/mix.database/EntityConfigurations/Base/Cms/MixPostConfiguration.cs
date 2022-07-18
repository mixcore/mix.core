namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixPostConfiguration<TConfig> : EntityBaseConfiguration<MixPost, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixPost> builder)
        {
            base.Configure(builder);
        }
    }
}
