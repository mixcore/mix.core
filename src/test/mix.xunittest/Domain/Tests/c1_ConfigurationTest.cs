using Mix.Database.Entities.Cms.v2;
using Mix.Portal.Domain.ViewModels;
using Mix.Xunittest.Domain.Base;
using Xunit;

namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 1 - Configuration")]
    public class c1_ConfigurationTest 
        : ViewModelTestBase<SharedMixCmsDbFixture, MixConfigurationViewModel, MixCmsContext, MixConfiguration, int>
        , IClassFixture<SharedMixCmsDbFixture>
    {
        public c1_ConfigurationTest(SharedMixCmsDbFixture fixture) : base(fixture)
        {
        }

        protected override MixConfigurationViewModel CreateSampleValue()
        {

            var data = new MixConfigurationViewModel()
            {
                DisplayName = "unit test",
                SystemName = "unit_test"
            };
            data.InitDefaultValues();
            return data;
        }
    }
}
