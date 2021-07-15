using Mix.Database.Entities.Cms.v2;
using Mix.Portal.Domain.ViewModels;
using Mix.Xunittest.Domain.Base;
using Xunit;

namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 3 - Post")]
    public class c3_PostTest 
        : ViewModelTestBase<SharedMixCmsDbFixture, MixPostViewModel, MixCmsContext, MixPost, int>
        , IClassFixture<SharedMixCmsDbFixture>
    {
        public c3_PostTest(SharedMixCmsDbFixture fixture) : base(fixture)
        {
        }

        protected override MixPostViewModel CreateSampleValue()
        {

            var data = new MixPostViewModel()
            {
                DisplayName = "unit test",
                SystemName = "unit_test"
            };
            return data;
        }
    }
}
