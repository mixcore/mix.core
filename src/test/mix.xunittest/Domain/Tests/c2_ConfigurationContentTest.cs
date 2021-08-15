using Mix.Database.Entities.Cms;
using Mix.Portal.Domain.ViewModels;
using Mix.Xunittest.Domain.Base;
using Xunit;

namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 2 - Configuration Content")]
    public class C2_ConfigurationContentTest
        : ViewModelTestBase<SharedMixCmsDbFixture, MixConfigurationContentViewModel, MixCmsContext, MixConfigurationContent, int>
        , IClassFixture<SharedMixCmsDbFixture>
    {
        public C2_ConfigurationContentTest(SharedMixCmsDbFixture fixture) : base(fixture)
        {
        }

        protected override MixConfigurationContentViewModel CreateSampleValue()
        {
            var data = new MixConfigurationContentViewModel()
            {
                ParentId = 1,
                Specificulture = "en-us",
                MixCultureId = 1,
                DisplayName = "unit test",
                Content = "test case 1"
            };
            data.InitDefaultValues();
            return data;
        }
    }
}
