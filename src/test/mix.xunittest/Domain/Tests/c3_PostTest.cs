using Mix.Lib.ViewModels;

namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 3 - Post")]
    public class C3_PostTest
        : ViewModelTestBase<SharedMixCmsDbFixture, MixPostViewModel, MixCmsContext, MixPost, int>
        , IClassFixture<SharedMixCmsDbFixture>
    {
        public C3_PostTest(SharedMixCmsDbFixture fixture) : base(fixture)
        {
        }

        protected override MixPostViewModel CreateSampleValue()
        {

            var data = new MixPostViewModel()
            {
                DisplayName = "unit test",
                MixTenantId = 1
            };
            return data;
        }
    }
}
