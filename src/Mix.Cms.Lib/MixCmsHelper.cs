using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib
{
    public class MixCmsHelper
    {
        public static FileViewModel LoadDataFile(string folder, string name)
        {
            return FileRepository.Instance.GetFile(name, folder, true, "[]");
        }

        public static string GetAssetFolder(string culture)
        {
            return $"/{MixConstants.Folder.FileFolder}/{MixConstants.Folder.TemplatesAssetFolder}/{MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, culture)}";
        }

        public static List<ViewModels.MixPages.ReadListItemViewModel> GetPage(IUrlHelper Url, string culture, MixEnums.CatePosition position, string activePath = "")
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
                    case MixPageType.Home:
                    case MixPageType.ListPost:
                    default:
                        cate.DetailsUrl = Url.RouteUrl("Alias", new { culture, seoName = cate.SeoName });
                        break;
                }
                cate.IsActived = (cate.DetailsUrl == activePath
                    || (cate.Type == MixPageType.Home && activePath == string.Format("/{0}/home", culture)));
                cate.Childs.ForEach((Action<ViewModels.MixPagePages.ReadViewModel>)(c =>
                {
                    c.IsActived = (
                    c.Page.DetailsUrl == activePath);
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
                    case MixPageType.Home:
                    case MixPageType.ListPost:
                    default:
                        cate.DetailsUrl = Url.RouteUrl("Alias", new { culture, seoName = cate.SeoName });
                        break;
                }

                cate.IsActived = (
                    cate.DetailsUrl == activePath || (cate.Type == MixPageType.Home && activePath == string.Format("/{0}/home", culture))
                    );

                cate.Childs.ForEach((Action<ViewModels.MixPagePages.ReadViewModel>)(c =>
                {
                    c.IsActived = (
                    c.Page.DetailsUrl == activePath);
                    cate.IsActived = cate.IsActived || c.IsActived;
                }));
            }
            return cates;
        }

        public static string GetRouterUrl(object routeValues, HttpRequest request, IUrlHelper Url)
        {
            Type objType = routeValues.GetType();
            string url = "";
            foreach (PropertyInfo prop in objType.GetProperties())
            {
                string name = prop.Name;
                var value = prop.GetValue(routeValues, null).ToString();
                url += $"/{value}";
            }
            return string.Format("{0}://{1}{2}", request.Scheme, request.Host,
                        url
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
            return double.TryParse(number, out _);
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

        public static void LogException(Exception ex)
        {
            string fullPath = string.Format($"{Environment.CurrentDirectory}/logs");
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string filePath = $"{fullPath}/{DateTime.Now.ToString("YYYYMMDD")}/log_exceptions.json";

            try
            {
                FileInfo file = new FileInfo(filePath);
                string content = "[]";
                if (file.Exists)
                {
                    using (StreamReader s = file.OpenText())
                    {
                        content = s.ReadToEnd();
                    }
                    File.Delete(filePath);
                }

                JArray arrExceptions = JArray.Parse(content);
                JObject jex = new JObject
                {
                    new JProperty("CreatedDateTime", DateTime.UtcNow),
                    new JProperty("Details", JObject.FromObject(ex))
                };
                arrExceptions.Add(jex);
                content = arrExceptions.ToString();

                using (var writer = File.CreateText(filePath))
                {
                    writer.WriteLine(content);
                }
            }
            catch
            {
                // File invalid
            }
        }

        public static System.Threading.Tasks.Task<ViewModels.MixModules.ReadMvcViewModel> GetModuleAsync(string name, string culture, IUrlHelper url = null)
        {
            var cacheKey = $"vm_{culture}_module_{name}_mvc";
            var module = new Domain.Core.ViewModels.RepositoryResponse<ViewModels.MixModules.ReadMvcViewModel>();

            // If not cached yet => load from db
            if (module == null || !module.IsSucceed)
            {
                module = ViewModels.MixModules.ReadMvcViewModel.GetBy(m => m.Name == name && m.Specificulture == culture);
            }

            // If load successful => load details
            if (module.IsSucceed)
            {
                if (url != null && module.Data.Posts != null)
                {
                    module.Data.Posts.Items.ForEach(a => { a.Post.DetailsUrl = url.RouteUrl("Post", new { id = a.PostId, seoName = a.Post.SeoName }); });
                }
                //await MixCacheService.SetAsync(cacheKey, module);
            }

            return Task.FromResult(module.Data);
        }

        public static async System.Threading.Tasks.Task<ViewModels.MixPages.ReadMvcViewModel> GetPageAsync(int id, string culture)
        {
            RepositoryResponse<ViewModels.MixPages.ReadMvcViewModel> getPage = null;
            if (getPage == null)
            {
                getPage = await ViewModels.MixPages.ReadMvcViewModel.Repository.GetSingleModelAsync(m => m.Id == id && m.Specificulture == culture);
            }

            return getPage.Data;
        }

        public static ViewModels.MixModules.ReadMvcViewModel GetModule(string name, string culture)
        {
            var module = ViewModels.MixModules.ReadMvcViewModel.GetBy(m => m.Name == name && m.Specificulture == culture);
            return module.Data;
        }

        public static ViewModels.MixPages.ReadMvcViewModel GetPage(int id, string culture)
        {
            var page = ViewModels.MixPages.ReadMvcViewModel.Repository.GetSingleModel(m => m.Id == id && m.Specificulture == culture);
            return page.Data;
        }

        public static async System.Threading.Tasks.Task<ViewModels.MixTemplates.ReadListItemViewModel> GetTemplateByPath(string templatePath)
        {
            string[] tmp = templatePath.Split('/');
            if (tmp[1].IndexOf('.') > 0)
            {
                tmp[1] = tmp[1].Substring(0, tmp[1].IndexOf('.'));
            }
            var getData = await ViewModels.MixTemplates.ReadListItemViewModel.Repository.GetFirstModelAsync(m => m.FolderType == tmp[0] && m.FileName == tmp[1]);

            return getData.Data;
        }

        public static async System.Threading.Tasks.Task<ViewModels.MixAttributeSetDatas.Navigation> GetNavigation(string name, string culture, IUrlHelper Url)
        {
            var navs = await ViewModels.MixAttributeSetDatas.Helper.FilterByKeywordAsync<ViewModels.MixAttributeSetDatas.NavigationViewModel>(culture, MixConstants.AttributeSetName.NAVIGATION, "equal", "name", name);
            var nav = navs.Data.FirstOrDefault()?.Nav;
            string action = Url.ActionContext.ActionDescriptor.RouteValues["action"];
            string activePath = string.Empty;
            switch (action)
            {
                case "Page":
                case "Post":
                    string seoName = Url.ActionContext.RouteData.Values["seoName"].ToString();
                    string id = Url.ActionContext.RouteData.Values["id"].ToString();
                    activePath = $"/{culture}/post/{id}/{seoName}";
                    break;

                case "Alias":
                    string alias = Url.ActionContext.HttpContext.Request.Query["alias"].ToString();
                    activePath = $"/{culture}/{alias}";
                    break;
            }
            if (nav != null && !string.IsNullOrEmpty(activePath))
            {
                foreach (var cate in nav.MenuItems)
                {
                    cate.IsActive = cate.Property<string>("uri") == activePath;

                    foreach (var item in cate.MenuItems)
                    {
                        item.IsActive = item.Property<string>("uri") == activePath;
                        cate.IsActive = cate.IsActive || item.IsActive;
                    }
                }
            }

            return nav;
        }
    }
}