using Mix.Database.Entities.Cms;
using Mix.Portal.Domain.ViewModels;
using Mix.Xunittest.Domain.Base;
using Xunit;

namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 4 - Post Content")]
    public class c4_PostContentTest
        : ViewModelTestBase<SharedMixCmsDbFixture, MixPostContentViewModel, MixCmsContext, MixPostContent, int>
        , IClassFixture<SharedMixCmsDbFixture>
    {
        public c4_PostContentTest(SharedMixCmsDbFixture fixture) : base(fixture)
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
                Excerpt = "test case 1"
            };
            data.InitDefaultValues();
            return data;
        }
    }
}
