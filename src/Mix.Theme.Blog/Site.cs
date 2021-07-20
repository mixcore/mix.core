using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Services;
using System;

namespace Mix.Theme.Blog
{
    public class Site
    {
        public string Logo { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string CoverImage { get; set; }

        public string Url { get; set; }

        public string ViewMode { get; set; }

        public bool IsAllowMembers { get; set; }

        public dynamic Post { get; set; }
        public dynamic NextPost { get; set; }
        public dynamic PreviousPost { get; set; }

        public MixNavigation Navigation { get; set; }

        public MixNavigation SocialNavigation { get; set; }

        public MixNavigation SecondaryNavigation { get; set; }

        public Site(string culture, IUrlHelper urlHelper)
        {
            Url = MixService.GetAppSetting<string>("Domain");
            Title = MixService.GetConfig<string>("SiteTitle", culture);
            Title = MixService.GetConfig<string>("SiteTitle", culture);
            Logo = MixService.GetConfig<string>("SiteLogo", culture);
            Description = MixService.GetConfig<string>("SiteDescription", culture);
            CoverImage = MixService.GetConfig<string>("SiteCoverImage", culture);
            IsAllowMembers = MixService.GetAppSetting<bool>("IsRegistration");

            Navigation = MixCmsHelper.GetNavigation("navigation", culture, urlHelper);
            SecondaryNavigation = MixCmsHelper.GetNavigation("secondary_navigation", culture, urlHelper);
            SocialNavigation = MixCmsHelper.GetNavigation("social_navigation", culture, urlHelper);
        }

        public string ImgUrl(string Url, string size)
        {
            string[] tmp = Url.Split('.');
            string rtn = "";
            for (int i = 0; i < tmp.Length; i++)
            {
                rtn += tmp[i];
                if (i < tmp.Length - 1)
                {
                    rtn += "_" + size + ".";
                }
            }
            return Url;
        }

        public int CountWord(string Content)
        {
            //Convert the string into an array of words
            string[] source = Content.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query.  Use ToLowerInvariant to match "data" and "Data"
            //var matchQuery = from word in source
            //                where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
            //                select word;

            // Count the matches, which executes the query.
            return source.Length;
        }

        public string CountReadingTime(string Content)
        {
            int totalSeconds = CountWord(Content) * 1 / 4;
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;
            string time = minutes + ":" + seconds;

            return time;
        }
    }
}