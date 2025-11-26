namespace Mix.Xunittest.Domain
{
    public class SharedMixCmsDbFixture
        : SharedDatabaseFixture<MixCmsContext>
    {
        protected override void SeedData(MixCmsContext dbContext)
        {
            dbContext.MixTenant.Add(
                   new MixTenant()
                   {
                       Id = 1,
                       SystemName = "test_site",
                       DisplayName = "Test Site"
                   });
            dbContext.MixCulture.Add(new MixCulture()
            {
                Specificulture = "en-us",
                TenantId = 1,
            });
            dbContext.MixConfiguration.Add(new MixConfiguration()
            {
                TenantId = 1,
                Id = 1,
            });
            dbContext.MixPost.Add(new MixPost()
            {
                TenantId = 1,
                Id = 1,
            });
            dbContext.SaveChanges();
        }
    }
}
