using Mix.Lib.ViewModels;

namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 4 - Post Content")]
    public class C4_PostContentTest
        : ViewModelTestBase<SharedMixCmsDbFixture, MixPostContentViewModel, MixCmsContext, MixPostContent, int>
        , IClassFixture<SharedMixCmsDbFixture>
    {
        public C4_PostContentTest(SharedMixCmsDbFixture fixture) : base(fixture)
        {
        }

        protected override MixPostContentViewModel CreateSampleValue()
        {
            var data = new MixPostContentViewModel()
            {
                ParentId = 1,
                Specificulture = "en-us",
                MixCultureId = 1,
                Title = "unit test",
                Content = "test case 1",
                Excerpt = "test case 1",
                TenantId = 1
            };
            data.InitDefaultValues();
            return data;
        }
    }
}
