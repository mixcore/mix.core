using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms.v2;
using Mix.Portal.Domain.ViewModels;
using Mix.XUnittest.Domain.Base;
using System;
using Xunit;

namespace Mix.XUnittest
{
    public class ConfigurationContentTest 
        : ViewModelTestBase<MixConfigurationContentViewModel, MixCmsContext, MixConfigurationContent, int>
    {
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

        protected override void Seed()
        {
            base.Seed();
            using(var ctx = new MixCmsContext(_connectionString, _dbProvider))
            {
                ctx.MixSite.Add(
                    new MixSite()
                    {
                        Id = 1,
                        SystemName = "test_site",
                        DisplayName = "Test Site"
                    });
                ctx.MixCulture.Add(new MixCulture()
                {
                    Specificulture = "en-us",
                    MixSiteId = 1,
                    DisplayName = "English"
                });
                ctx.MixConfiguration.Add(new MixConfiguration()
                {
                    MixSiteId = 1,
                    Id = 1,
                    DisplayName = "Config 1",
                    SystemName = "config_1"
                });
                ctx.SaveChanges();
            }
        }
    }
}
