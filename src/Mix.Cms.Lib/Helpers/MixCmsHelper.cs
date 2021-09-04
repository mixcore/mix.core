using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mix.Infrastructure.Repositories;
using System.Web;
using MixDatas = Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using MixPagePosts = Mix.Cms.Lib.ViewModels.MixPagePosts;
using MixPosts = Mix.Cms.Lib.ViewModels.MixPosts;
using System.Collections.Specialized;

namespace Mix.Cms.Lib.Helpers
{
    public class MixCmsHelper
    {
        public static string GetSEOString(string input)
        {
            return SeoHelper.GetSEOString(input);
        }

        public static FileViewModel LoadDataFile(string folder, string name)
        {
            return MixFileRepository.Instance.GetFile(name, folder, true, "[]");
        }

        public static string GetAssetFolder(string culture = null)
        {
            culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            return $"{MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain)}/" +
                $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, culture)}/assets";
        }

        public static string GetUploadFolder(string culture = null)
        {
            culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            return $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, culture)}/uploads/" +
                $"{DateTime.UtcNow.ToString(MixConstants.CONST_UPLOAD_FOLDER_DATE_FORMAT)}";
        }

        public static string GetTemplateFolder(string culture)
        {
            return $"/{MixFolders.TemplatesFolder}/{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, culture)}";
        }

        public static T Property<T>(JObject obj, string fieldName)
        {
            if (obj != null && obj.ContainsKey(fieldName) && obj[fieldName] != null)
            {
                return obj.Value<T>(fieldName);
            }
            else
            {
                return default(T);
            }
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

            return string.Format("{0}://{1}{2}", request.Scheme, request.Host, url);
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

        public static Task<ViewModels.MixModules.ReadMvcViewModel> GetModuleAsync(string name, string culture = null, IUrlHelper url = null)
        {
            var cacheKey = $"vm_{culture}_module_{name}_mvc";
            var module = new RepositoryResponse<ViewModels.MixModules.ReadMvcViewModel>();
            culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            // If not cached yet => load from db
            if (module == null || !module.IsSucceed)
            {
                module = ViewModels.MixModules.ReadMvcViewModel.GetBy(m => m.Name == name && m.Specificulture == culture);
            }

            // If load successful => load details
            return Task.FromResult(module.Data);
        }

        internal static string GetDetailsUrl(string specificulture, string path)
        {
            return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain).TrimEnd('/')
                    + (specificulture != MixService.Instance.DefaultCulture ? $"/{specificulture}" : string.Empty)
                    + path;
        }

        public static async Task<ViewModels.MixPages.ReadMvcViewModel> GetPageAsync(int id, string culture)
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

        public static async Task<ViewModels.MixTemplates.ReadViewModel> GetTemplateByPath(string themeName, string templatePath)
        {
            string[] tmp = templatePath.Split('/');
            if (tmp[1].IndexOf('.') > 0)
            {
                tmp[1] = tmp[1].Substring(0, tmp[1].IndexOf('.'));
            }
            var getData = await ViewModels.MixTemplates.ReadViewModel.Repository.GetFirstModelAsync(
                m => m.ThemeName == themeName
                && m.FolderType == tmp[0]
                && m.FileName == tmp[1]);

            return getData.Data;
        }

        public static async Task<MixNavigation> GetNavigationAsync(string name, string culture, IUrlHelper Url)
        {
            var navs = await MixDatas.Helper
                .FilterByKeywordAsync<MixDatas.NavigationViewModel>(culture, MixConstants.MixDatabaseName.NAVIGATION, "equal", "name", name);

            var nav = navs.Data?.FirstOrDefault()?.Nav;
            string activePath = Url.ActionContext.HttpContext.Request.Path;

            if (nav != null)
            {
                foreach (var cate in nav.MenuItems)
                {
                    cate.IsActive = cate.Uri == activePath;
                    if (cate.IsActive)
                    {
                        nav.ActivedMenuItem = cate;
                        nav.ActivedMenuItems.Add(cate);
                    }

                    foreach (var item in cate.MenuItems)
                    {
                        item.IsActive = item.Uri == activePath;
                        if (item.IsActive)
                        {
                            nav.ActivedMenuItem = item;
                            nav.ActivedMenuItems.Add(cate);
                            nav.ActivedMenuItems.Add(item);
                        }
                        cate.IsActive = cate.IsActive || item.IsActive;
                    }
                }
            }

            return nav;
        }

        public static MixNavigation GetNavigation(string name, string culture, IUrlHelper Url)
        {
            var navs = MixDatas.Helper
                .FilterByKeyword<MixDatas.NavigationViewModel>(culture, MixConstants.MixDatabaseName.NAVIGATION, "equal", "name", name);

            var nav = navs.Data?.FirstOrDefault()?.Nav;
            string activePath = Url.ActionContext.HttpContext.Request.Path;

            if (nav != null)
            {
                foreach (var cate in nav.MenuItems)
                {
                    cate.IsActive = cate.Uri == activePath;
                    if (cate.IsActive)
                    {
                        nav.ActivedMenuItem = cate;
                        nav.ActivedMenuItems.Add(cate);
                    }

                    foreach (var item in cate.MenuItems)
                    {
                        item.IsActive = item.Uri == activePath;
                        if (item.IsActive)
                        {
                            nav.ActivedMenuItem = item;
                            nav.ActivedMenuItems.Add(cate);
                            nav.ActivedMenuItems.Add(item);
                        }
                        cate.IsActive = cate.IsActive || item.IsActive;
                    }
                }
            }

            return nav;
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetListPostByAdditionalField<TView>(
            string fieldName,
            object fieldValue,
            string culture,
            MixDataType dataType,
            MixCompareOperatorKind filterType = MixCompareOperatorKind.Equal,
            string orderByPropertyName = null,
            DisplayDirection direction = DisplayDirection.Asc,
            int? pageSize = null,
            int? pageIndex = null,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null) where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                    {
                        PageIndex = pageIndex.HasValue ? pageIndex.Value : 0,
                        PageSize = pageSize
                    }
                };
                // Get Value Predicate By Type
                Expression<Func<MixDatabaseDataValue, bool>> valPredicate =
                    m => m.MixDatabaseName == MixConstants.MixDatabaseName.ADDITIONAL_COLUMN_POST
                    && m.MixDatabaseColumnName == fieldName;

                var pre = GetValuePredicate(fieldValue.ToString(), filterType, dataType);
                if (pre != null)
                {
                    valPredicate = valPredicate.AndAlso(pre);
                }

                var query = context.MixDatabaseDataValue.Where(valPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();

                var relatedQuery = context.MixDatabaseDataAssociation.Where(
                    m => m.ParentType == MixDatabaseParentType.Post
                    && m.Specificulture == culture
                    && dataIds.Any(d => d == m.DataId));

                var postIds = relatedQuery.Select(m => int.Parse(m.ParentId)).Distinct().AsEnumerable().ToList();

                result = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetModelListByAsync(
                    m => m.Specificulture == culture && postIds.Any(p => p == m.Id),
                    orderByPropertyName,
                    direction,
                    pageSize ?? 100,
                    pageIndex ?? 0,
                    null,
                    null,
                    context,
                    transaction);

                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        private static Expression<Func<MixDatabaseDataValue, bool>> GetValuePredicate(string fieldValue, MixCompareOperatorKind filterType, MixDataType dataType)
        {
            Expression<Func<MixDatabaseDataValue, bool>> valPredicate = null;
            switch (dataType)
            {
                case MixDataType.Date:
                case MixDataType.Time:
                    if (DateTime.TryParse(fieldValue, out DateTime dtValue))
                    {
                        valPredicate = FilterObjectSet<MixDatabaseDataValue, DateTime>("DateTimeValue", dtValue, filterType);
                    }
                    break;

                case MixDataType.Double:
                    if (double.TryParse(fieldValue, out double dbValue))
                    {
                        valPredicate = FilterObjectSet<MixDatabaseDataValue, double>("DoubleValue", dbValue, filterType);
                    }
                    break;

                case MixDataType.Boolean:
                    if (bool.TryParse(fieldValue, out bool boolValue))
                    {
                        valPredicate = FilterObjectSet<MixDatabaseDataValue, bool>("BooleanValue", boolValue, filterType);
                    }
                    break;

                case MixDataType.Integer:
                    if (int.TryParse(fieldValue, out int intValue))
                    {
                        valPredicate = FilterObjectSet<MixDatabaseDataValue, int>("IntegerValue", intValue, filterType);
                    }
                    break;

                case MixDataType.Reference:
                    break;

                case MixDataType.Duration:
                case MixDataType.Custom:
                case MixDataType.DateTime:
                case MixDataType.PhoneNumber:
                case MixDataType.Text:
                case MixDataType.Html:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Upload:
                case MixDataType.Color:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                case MixDataType.QRCode:
                default:
                    valPredicate = FilterObjectSet<MixDatabaseDataValue, string>("StringValue", fieldValue, filterType);
                    break;
            }

            return valPredicate;
        }

        public static Expression<Func<TModel, bool>> FilterObjectSet<TModel, T>(string propName, T data2, MixCompareOperatorKind filterType)
        {
            Type type = typeof(TModel);
            var par = Expression.Parameter(type, "model");

            Type fieldPropertyType;
            Expression fieldPropertyExpression;

            FieldInfo fieldInfo = type.GetField(propName);

            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = type.GetProperty(propName);

                if (propertyInfo == null)
                {
                    throw new Exception();
                }

                fieldPropertyType = propertyInfo.PropertyType;
                fieldPropertyExpression = Expression.Property(par, propertyInfo);
            }
            else
            {
                fieldPropertyType = fieldInfo.FieldType;
                fieldPropertyExpression = Expression.Field(par, fieldInfo);
            }

            BinaryExpression eq = null;
            switch (filterType)
            {
                case MixCompareOperatorKind.Equal:
                    eq = Expression.Equal(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;

                case MixCompareOperatorKind.LessThan:
                    eq = Expression.LessThan(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;

                case MixCompareOperatorKind.GreaterThan:
                    eq = Expression.GreaterThan(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;

                case MixCompareOperatorKind.LessThanOrEqual:
                    eq = Expression.LessThanOrEqual(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;

                case MixCompareOperatorKind.GreaterThanOrEqual:
                    eq = Expression.GreaterThanOrEqual(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;

                case MixCompareOperatorKind.InRange:
                    var method = typeof(string).GetMethod("Contains");
                    var call = Expression.Call(par, method, Expression.Constant(data2, typeof(string)));
                    return Expression.Lambda<Func<TModel, bool>>(call, par);

                default:
                    break;
            }
            return Expression.Lambda<Func<TModel, bool>>(eq, par);
        }

        public async static Task<RepositoryResponse<PaginationModel<TView>>> GetPostlistByMeta<TView>(
            HttpContext context,
            string keyword = null,
            string culture = null,
            string type = MixConstants.MixDatabaseName.SYSTEM_TAG,
            int? pageSize = null,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null) where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            keyword ??= context.Request.Query["Keyword"];

            PagingRequest pagingRequest = new PagingRequest(context.Request);
            if (pageSize.HasValue)
            {
                pagingRequest.PageSize = pageSize.Value;
            }
            return await MixPosts.Helper.GetModelistByMeta<TView>(
                type,
                keyword,
                MixDatabaseNames.ADDITIONAL_COLUMN_POST,
                pagingRequest,
                culture,
                _context,
                _transaction);
        }

        public async static Task<RepositoryResponse<PaginationModel<TView>>> GetPostlistByAdditionalField<TView>(
            string fieldName,
            string value,
            string culture,
            string orderByPropertyName = null,
            DisplayDirection direction = DisplayDirection.Asc,
            int? pageSize = null,
            int? pageIndex = 0,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null) where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            int maxPageSize = MixService.GetAppSetting<int>("MaxPageSize");
            string orderBy = MixService.GetAppSetting<string>("OrderBy");
            pageSize = (pageSize > 0 && pageSize < maxPageSize) ? pageSize : maxPageSize;
            pageIndex = (pageIndex >= 0) ? pageIndex : 0;

            return await MixPosts.Helper.SearchPostByField<TView>(
                fieldName,
                value,
                culture,
                orderByPropertyName ?? orderBy,
                direction,
                pageSize,
                pageIndex - 1,
                _context,
                _transaction);
        }

        public static async Task<RepositoryResponse<PaginationModel<MixPagePosts.ReadViewModel>>> GetPostListByPageId(
            HttpContext context,
            int pageId,
            string keyword = null,
            string culture = null,
            string orderBy = "CreatedDateTime",
            DisplayDirection direction = DisplayDirection.Desc,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null)
        {
            int.TryParse(context.Request.Query[MixRequestQueryKeywords.Page], out int page);
            int.TryParse(context.Request.Query[MixRequestQueryKeywords.PageSize], out int pageSize);
            page = (page > 0) ? page : 1;
            var result = await MixPosts.Helper.GetPostListByPageId<MixPagePosts.ReadViewModel>(
                pageId, keyword, culture,
                orderBy, direction, pageSize, page - 1, _context, _transaction);
            result.Data.Items.ForEach(m => m.LoadPost(_context, _transaction));
            return result;
        }

        public static async Task<RepositoryResponse<PaginationModel<MixDatas.ReadMvcViewModel>>> GetAttributeDataListBySet(
            HttpContext context,
            string mixDatabaseName,
            string culture = null,
            DisplayDirection direction = DisplayDirection.Desc,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null)
        {
            var result = await MixDatas.Helper.FilterByKeywordAsync<MixDatas.ReadMvcViewModel>(context.Request, culture, mixDatabaseName);
            return result;
        }

        public static string TranslateUrl(string url, string srcCulture, string destCulture)
        {
            return url.Contains($"/{srcCulture}")
                ? url.Replace(srcCulture, destCulture)
                : $"/{destCulture}{url}";
        }

        public static string BuildUrl(HttpContext context, string key, string value)
        {
            var nameValueCollection = new NameValueCollection
            {
                { key, value }
            };
            var request = context.Request;
            var uri = string.Format("{0}://{1}{2}", request.Scheme, request.Host, request.Path);
            var queryString = HttpUtility.ParseQueryString(request.QueryString.Value);

            var uriBuilder = new UriBuilder(uri);

            if (context.Request.Query.ContainsKey(key))
            {
                queryString.Set(key, value);
            }
            else
            {
                queryString.Add(nameValueCollection);
            }
            uriBuilder.Query = queryString.ToString();
            return uriBuilder.ToString();
        }
    }
}