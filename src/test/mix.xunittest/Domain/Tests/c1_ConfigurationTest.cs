namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 1 - Configuration")]
    public class Step_1_ConfigurationTest 
        : ViewModelTestBase<SharedMixCmsDbFixture, MixConfigurationViewModel, MixCmsContext, MixConfiguration, int>
        , IClassFixture<SharedMixCmsDbFixture>
    {
        public Step_1_ConfigurationTest(SharedMixCmsDbFixture fixture) : base(fixture)
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
