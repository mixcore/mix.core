using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms.v2;
using Mix.Portal.Domain.ViewModels;
using Mix.XUnit.Domain.Base;
using System;
using Xunit;

namespace Mix.XUnit.Domain.Tests
{
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
