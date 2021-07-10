using Mix.Database.Entities.Cms.v2;
using Mix.Portal.Domain.ViewModels;
using Mix.Xunittest.Domain.Base;
using Xunit;

namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 2 - Configuration Content")]
    public class ConfigurationContentTest
        : ViewModelTestBase<SharedMixCmsDbFixture, MixConfigurationContentViewModel, MixCmsContext, MixConfigurationContent, int>
        , IClassFixture<SharedMixCmsDbFixture>
    {
        public ConfigurationContentTest(SharedMixCmsDbFixture fixture) : base(fixture)
        {
        }

        protected override MixConfigurationContentViewModel CreateSampleValue()
        {
            var data = new MixConfigurationContent()
            {
                MixConfigurationId = 1,
                Specificulture = "en-us",
                MixCultureId = 1,
                DisplayName = "unit test",
                Content = "test case 1"
            };
            return new MixConfigurationContentViewModel(data);
        }
    }
}
