using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Services;
using Mix.Heart.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Theme.Blog
{
    public class Site
    {
        public Site(string culture, IUrlHelper Url)
        {

            Title = MixService.GetConfig<string>("SiteTitle", culture);
            Logo = MixService.GetConfig<string>("SiteLogo", culture);
            Description = MixService.GetConfig<string>("SiteDescription", culture);
            CoverImage = MixService.GetConfig<string>("SiteCoverImage", culture);
            //Navigation = await MixCmsHelper.GetNavigation("navigation", culture, Url);
            //SocialNavigation = await MixCmsHelper.GetNavigation("social_navigation", culture, Url);
            IsAllowMembers = MixService.GetConfig<bool>("IsRegistration");

            //Navigation = await MixCmsHelper.GetNavigation("navigation", culture, Url);
            //SecondaryNavigation = await MixCmsHelper.GetNavigation("secondary_navigation", culture, Url);
            //SocialNavigation = await MixCmsHelper.GetNavigation("social_navigation", culture, Url);

        }
        public string Logo { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string CoverImage { get; set; }

        public string Url { get; set; }

        public string ViewMode { get; set; }

        public bool IsAllowMembers { get; set; }

        public dynamic Post { get; set; }

        public MixNavigation Navigation { get; set; }

        public MixNavigation SocialNavigation { get; set; }

        public MixNavigation SecondaryNavigation { get; set; }
    }
}
