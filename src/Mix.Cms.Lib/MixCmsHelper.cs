using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib
{
    public class MixCmsHelper
    {
        public static List<ViewModels.MixPages.ReadListItemViewModel> GetCategory(IUrlHelper Url, string culture, MixEnums.CatePosition position, string activePath = "")
        {
            var getTopCates = ViewModels.MixPages.ReadListItemViewModel.Repository.GetModelListBy
            (c => c.Specificulture == culture && c.MixPagePosition.Any(
              p => p.PositionId == (int)position)
            );
            var cates = getTopCates.Data ?? new List<ViewModels.MixPages.ReadListItemViewModel>();
            activePath = activePath.ToLower();
            foreach (var cate in cates)
            {
                switch (cate.Type)
                {
                    case MixPageType.Blank:
                        foreach (var child in cate.Childs)
                        {
                            child.DetailsUrl = Url.RouteUrl("Page", new { culture, seoName = child.SeoName });
                        }
                        break;

                    case MixPageType.StaticUrl:
                        cate.DetailsUrl = cate.StaticUrl;
                        break;

                    case MixPageType.Home:
                    case MixPageType.ListArticle:
                    case MixPageType.Article:
                    case MixPageType.Modules:
                    default:
                        cate.DetailsUrl = Url.RouteUrl("Page", new { culture, seoName = cate.SeoName });
                        break;
                }
                cate.IsActived = (cate.DetailsUrl == activePath
                    || (cate.Type == MixPageType.Home && activePath == string.Format("/{0}/home", culture)));
                cate.Childs.ForEach((Action<ViewModels.MixPages.ReadListItemViewModel>)(c =>
                {
                    c.IsActived = (
                    c.DetailsUrl == activePath);
                    cate.IsActived = cate.IsActived || c.IsActived;
                }));
            }
            return cates;
        }

        public static List<ViewModels.MixPages.ReadListItemViewModel> GetCategory(IUrlHelper Url, string culture, MixPageType cateType, string activePath = "")
        {
            var getTopCates = ViewModels.MixPages.ReadListItemViewModel.Repository.GetModelListBy
            (c => c.Specificulture == culture && c.Type == (int)cateType
            );
            var cates = getTopCates.Data ?? new List<ViewModels.MixPages.ReadListItemViewModel>();
            activePath = activePath.ToLower();
            foreach (var cate in cates)
            {
                switch (cate.Type)
                {
                    case MixPageType.Blank:
                        foreach (var child in cate.Childs)
                        {
                            child.DetailsUrl = Url.RouteUrl("Page", new { culture, pageName = child.SeoName });
                        }
                        break;

                    case MixPageType.StaticUrl:
                        cate.DetailsUrl = cate.StaticUrl;
                        break;

                    case MixPageType.Home:
                    case MixPageType.ListArticle:
                    case MixPageType.Article:
                    case MixPageType.Modules:
                    default:
                        cate.DetailsUrl = Url.RouteUrl("Page", new { culture, pageName = cate.SeoName });
                        break;
                }

                cate.IsActived = (
                    cate.DetailsUrl == activePath || (cate.Type == MixPageType.Home && activePath == string.Format("/{0}/home", culture))
                    );

                cate.Childs.ForEach((Action<ViewModels.MixPages.ReadListItemViewModel>)(c =>
                {
                    c.IsActived = (
                    c.DetailsUrl == activePath);
                    cate.IsActived = cate.IsActived || c.IsActived;
                }));
            }
            return cates;
        }


        public static string GetRouterUrl(string routerName, object routeValues, HttpRequest request, IUrlHelper Url)
        {
            return string.Format("{0}://{1}{2}", request.Scheme, request.Host,
                        Url.RouteUrl(routerName, routeValues)
                        );
        }


        public static string FormatPrice(double? price, string oldPrice = "0")
        {
            string strPrice = price?.ToString();
            if (string.IsNullOrEmpty(strPrice))
            {
                return "0";
            }
            string s1 = strPrice.Replace(",", string.Empty);
            if (CheckIsPrice(s1))
            {
                Regex rgx = new Regex("(\\d+)(\\d{3})");
                while (rgx.IsMatch(s1))
                {
                    s1 = rgx.Replace(s1, "$1" + "," + "$2");
                }
                return s1;
            }
            return oldPrice;
        }
        public static bool CheckIsPrice(string number)
        {
            if (number == null)
            {
                return false;
            }
            number = number.Replace(",", "");

            return double.TryParse(number, out double t);
        }

        public static double ReversePrice(string formatedPrice)
        {
            try
            {
                if (string.IsNullOrEmpty(formatedPrice))
                {
                    return 0;
                }
                return double.Parse(formatedPrice.Replace(",", string.Empty));
            }
            catch
            {
                return 0;
            }
        }
    }
}
