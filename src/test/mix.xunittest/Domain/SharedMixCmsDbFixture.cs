using Mix.Database.Entities.Cms.v2;
using Mix.Xunittest.Domain.Base;

namespace Mix.Xunittest.Domain
{
    public class SharedMixCmsDbFixture
        : SharedDatabaseFixture<MixCmsContext>
    {
        protected override void SeedData(MixCmsContext dbContext)
        {
            dbContext.MixSite.Add(
                   new MixSite()
                   {
                       Id = 1,
                       SystemName = "test_site",
                       DisplayName = "Test Site"
                   });
            dbContext.MixCulture.Add(new MixCulture()
            {
                Specificulture = "en-us",
                MixSiteId = 1,
                DisplayName = "English"
            });
            dbContext.MixConfiguration.Add(new MixConfiguration()
            {
                MixSiteId = 1,
                Id = 1,
                DisplayName = "Config 1",
                SystemName = "config_1"
            });
            dbContext.MixPost.Add(new MixPost()
            {
                MixSiteId = 1,
                Id = 1,
                DisplayName = "Config 1",
                SystemName = "config_1"
            });
            dbContext.SaveChanges();
        }
    }
}
