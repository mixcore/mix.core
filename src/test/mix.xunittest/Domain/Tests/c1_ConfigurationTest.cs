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
                SystemName = "unit_test",
                MixTenantId = 1
            };
            data.InitDefaultValues();
            return data;
        }
    }
}
