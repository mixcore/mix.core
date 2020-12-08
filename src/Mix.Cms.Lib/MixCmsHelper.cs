using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib
{
    public class MixCmsHelper
    {
        public static string GetSEOString(string input)
        {
            return SeoHelper.GetSEOString(input);
        }
        public static FileViewModel LoadDataFile(string folder, string name)
        {
            return FileRepository.Instance.GetFile(name, folder, true, "[]");
        }

        public static string GetAssetFolder(string culture)
        {
            return $"{MixService.GetConfig<string>("Domain")}/{MixConstants.Folder.FileFolder}/{MixConstants.Folder.TemplatesAssetFolder}/{MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, culture)}/assets";
        }
        public static string GetTemplateFolder(string culture)
        {
            return $"/{MixConstants.Folder.TemplatesFolder}/{MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, culture)}";
        }
        public static T Property<T>(JObject obj,  string fieldName)
        {
            if (obj != null  && obj.ContainsKey(fieldName)  && obj[fieldName] != null)
            {
                return obj.Value<T>(fieldName);
            }
            else
            {
                return default(T);
            }
        }
        //public static List<ViewModels.MixPages.ReadListItemViewModel> GetPage(IUrlHelper Url, string culture, MixEnums.CatePosition position, string activePath = "")
        //{
        //    var getTopCates = ViewModels.MixPages.ReadListItemViewModel.Repository.GetModelListBy
        //    (c => c.Specificulture == culture && c.MixPagePosition.Any(
        //      p => p.PositionId == (int)position)
        //    );
        //    var cates = getTopCates.Data ?? new List<ViewModels.MixPages.ReadListItemViewModel>();
        //    activePath = activePath.ToLower();
        //    foreach (var cate in cates)
        //    {
        //        switch (cate.Type)
        //        {
        //            case MixPageType.Home:
        //            case MixPageType.ListPost:
        //            default:
        //                cate.DetailsUrl = Url.RouteUrl("Alias", new { culture, seoName = cate.SeoName });
        //                break;
        //        }
        //        cate.IsActived = (cate.DetailsUrl == activePath
        //            || (cate.Type == MixPageType.Home && activePath == string.Format("/{0}/home", culture)));
        //        cate.Childs.ForEach((Action<ViewModels.MixPagePages.ReadViewModel>)(c =>
        //        {
        //            c.IsActived = (
        //            c.Page.DetailsUrl == activePath);
        //            cate.IsActived = cate.IsActived || c.IsActived;
        //        }));
        //    }
        //    return cates;
        //}

        public static List<ViewModels.MixPages.ReadListItemViewModel> GetCategory(IUrlHelper Url, string culture, MixPageType cateType, string activePath = "")
        {
            var getTopCates = ViewModels.MixPages.ReadListItemViewModel.Repository.GetModelListBy
            (c => c.Specificulture == culture && c.Type == cateType.ToString()
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

        public static async System.Threading.Tasks.Task<ViewModels.MixTemplates.ReadViewModel> GetTemplateByPath(string themeName, string templatePath)
        {
            string[] tmp = templatePath.Split('/');
            if (tmp[1].IndexOf('.') > 0)
            {
                tmp[1] = tmp[1].Substring(0, tmp[1].IndexOf('.'));
            }
            var getData = await ViewModels.MixTemplates.ReadViewModel.Repository.GetFirstModelAsync(
                m => m.ThemeName == themeName && m.FolderType == tmp[0] && m.FileName == tmp[1]);

            return getData.Data;
        }

        public static async System.Threading.Tasks.Task<ViewModels.MixAttributeSetDatas.Navigation> GetNavigation(
            string name, string culture, IUrlHelper Url)
        {
            var navs = await ViewModels.MixAttributeSetDatas.Helper.FilterByKeywordAsync<ViewModels.MixAttributeSetDatas.NavigationViewModel>(culture, MixConstants.AttributeSetName.NAVIGATION, "equal", "name", name);
            var nav = navs.Data?.FirstOrDefault()?.Nav;
            string activePath = Url.ActionContext.HttpContext.Request.Path;
            //string controller = Url.ActionContext.ActionDescriptor.RouteValues["controller"];
            //switch (controller)
            //{
            //    case "Page":
            //    case "Post":
            //        string seoName = Url.ActionContext.RouteData.Values["seoName"].ToString();
            //        string id = Url.ActionContext.RouteData.Values["id"].ToString();
            //        activePath = $"/{culture}/post/{id}/{seoName}";
            //        break;

            //    case "Home":
            //        string alias = Url.ActionContext.HttpContext.Request.Query["alias"].ToString();
            //        activePath = $"/{culture}/{alias}";
            //        break;
            //}
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

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetListPostByAddictionalField<TView>(
            string fieldName, object fieldValue, string culture, MixEnums.MixDataType dataType
            , MixEnums.CompareType filterType = MixEnums.CompareType.Eq
            , string orderByPropertyName = null, Heart.Enums.MixHeartEnums.DisplayDirection direction = Heart.Enums.MixHeartEnums.DisplayDirection.Asc, int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
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
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.AttributeSetName == MixConstants.AttributeSetName.ADDITIONAL_FIELD_POST
                   && m.AttributeFieldName == fieldName;

                var pre = GetValuePredicate(fieldValue.ToString(), filterType, dataType);
                if (pre != null)
                {
                    valPredicate = Mix.Heart.Helpers.ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                }

                var query = context.MixAttributeSetValue.Where(valPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                var relatedQuery = context.MixRelatedAttributeData.Where(
                         m => m.ParentType == MixEnums.MixAttributeSetDataType.Post.ToString() && m.Specificulture == culture
                            && dataIds.Any(d => d == m.DataId));
                var postIds = relatedQuery.Select(m => int.Parse(m.ParentId)).Distinct().AsEnumerable().ToList();
                result = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetModelListByAsync(
                    m => m.Specificulture == culture && postIds.Any(p => p == m.Id)
                    , orderByPropertyName, direction
                    , pageSize ?? 100, pageIndex ?? 0
                    , null, null
                    , context, transaction);
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

        private static Expression<Func<MixAttributeSetValue, bool>> GetValuePredicate(string fieldValue
            , MixEnums.CompareType filterType, MixEnums.MixDataType dataType)
        {
            Expression<Func<MixAttributeSetValue, bool>> valPredicate = null;
            switch (dataType)
            {
                case MixEnums.MixDataType.Date:
                case MixEnums.MixDataType.Time:
                    if (DateTime.TryParse(fieldValue, out DateTime dtValue))
                    {
                        valPredicate = FilterObjectSet<MixAttributeSetValue, DateTime>("DateTimeValue", dtValue, filterType);
                    }
                    break;
                case MixEnums.MixDataType.Double:
                    if (double.TryParse(fieldValue, out double dbValue))
                    {
                        valPredicate = FilterObjectSet<MixAttributeSetValue, double>("DoubleValue", dbValue, filterType);
                    }
                    break;
                case MixEnums.MixDataType.Boolean:
                    if (bool.TryParse(fieldValue, out bool boolValue))
                    {
                        valPredicate = FilterObjectSet<MixAttributeSetValue, bool>("BooleanValue", boolValue, filterType);
                    }
                    break;
                case MixEnums.MixDataType.Integer:
                    if (int.TryParse(fieldValue, out int intValue))
                    {
                        valPredicate = FilterObjectSet<MixAttributeSetValue, int>("IntegerValue", intValue, filterType);
                    }
                    break;
                case MixEnums.MixDataType.Reference:
                    break;
                case MixEnums.MixDataType.Duration:
                case MixEnums.MixDataType.Custom:
                case MixEnums.MixDataType.DateTime:
                case MixEnums.MixDataType.PhoneNumber:
                case MixEnums.MixDataType.Text:
                case MixEnums.MixDataType.Html:
                case MixEnums.MixDataType.MultilineText:
                case MixEnums.MixDataType.EmailAddress:
                case MixEnums.MixDataType.Password:
                case MixEnums.MixDataType.Url:
                case MixEnums.MixDataType.ImageUrl:
                case MixEnums.MixDataType.CreditCard:
                case MixEnums.MixDataType.PostalCode:
                case MixEnums.MixDataType.Upload:
                case MixEnums.MixDataType.Color:
                case MixEnums.MixDataType.Icon:
                case MixEnums.MixDataType.VideoYoutube:
                case MixEnums.MixDataType.TuiEditor:
                case MixEnums.MixDataType.QRCode:
                default:
                    valPredicate = FilterObjectSet<MixAttributeSetValue, string>("StringValue", fieldValue, filterType);
                    break;
            }

            return valPredicate;
        }

        public static Expression<Func<TModel, bool>> FilterObjectSet<TModel, T>(string propName, T data2, MixEnums.CompareType filterType)
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
                case MixEnums.CompareType.Eq:
                    eq = Expression.Equal(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.Lt:
                    eq = Expression.LessThan(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.Gt:
                    eq = Expression.GreaterThan(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.Lte:
                    eq = Expression.LessThanOrEqual(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.Gte:
                    eq = Expression.GreaterThanOrEqual(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.In:
                    var method = typeof(string).GetMethod("Contains");
                    var call = Expression.Call(par, method, Expression.Constant(data2, typeof(string)));
                    return Expression.Lambda<Func<TModel, bool>>(call, par);
                default:
                    break;
            }
            return Expression.Lambda<Func<TModel, bool>>(eq, par);
        }

        public async static Task<RepositoryResponse<PaginationModel<TView>>> GetPostlistByMeta<TView>(

            HttpContext context
            , string culture, string type = MixConstants.AttributeSetName.SYSTEM_TAG
            , string orderByPropertyName = "CreatedDateTime", Heart.Enums.MixHeartEnums.DisplayDirection direction = MixHeartEnums.DisplayDirection.Desc
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            int maxPageSize = MixService.GetConfig<int>("MaxPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(context.Request.Query["page"], out int page);
            int.TryParse(context.Request.Query["pageSize"], out int pageSize);
            pageSize = (pageSize > 0 && pageSize < maxPageSize) ? pageSize : maxPageSize;
            page = (page > 0) ? page : 1;

            return await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetModelistByMeta<TView>(
                type, context.Request.Query["keyword"],
                culture, orderByPropertyName, direction, pageSize, page - 1, _context, _transaction);
        }

        public async static Task<RepositoryResponse<PaginationModel<TView>>> GetPostlistByAddictionalField<TView>(

            string fieldName, string value, string culture
            , string orderByPropertyName = null, Heart.Enums.MixHeartEnums.DisplayDirection direction = MixHeartEnums.DisplayDirection.Asc
            , int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            int maxPageSize = MixService.GetConfig<int>("MaxPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            pageSize = (pageSize > 0 && pageSize < maxPageSize) ? pageSize : maxPageSize;
            pageIndex = (pageIndex >= 0) ? pageIndex : 0;

            return await Mix.Cms.Lib.ViewModels.MixPosts.Helper.GetModelistByAddictionalField<TView>(
                fieldName, value,
                culture, orderByPropertyName ?? orderBy, direction, pageSize, pageIndex - 1, _context, _transaction);
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetAttributeDataByParent<TView>(
            string culture, string attributeSetName,
            string parentId, MixEnums.MixAttributeSetDataType parentType,
            string orderBy = "CreatedDateTime", Heart.Enums.MixHeartEnums.DisplayDirection direction = MixHeartEnums.DisplayDirection.Desc,
            int? pageSize = null, int? pageIndex = 0,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixAttributeSetData, TView>
        {
            return await ViewModels.MixAttributeSetDatas.Helper.GetAttributeDataByParent<TView>(
                culture, attributeSetName,
                parentId, parentType, orderBy, direction, pageSize, pageIndex, _context, _transaction);
        }
        
        public static async Task<RepositoryResponse<PaginationModel<Lib.ViewModels.MixPagePosts.ReadViewModel>>> GetPostListByPageId(
            HttpContext context
            , int pageId
            , string keyword = null
            , string culture = null
            , string orderBy = "CreatedDateTime"
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = MixHeartEnums.DisplayDirection.Desc
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null
            )
        {
            int.TryParse(context.Request.Query["page"], out int page);
            int.TryParse(context.Request.Query["pageSize"], out int pageSize);
            page = (page > 0) ? page : 1;
            var result = await ViewModels.MixPosts.Helper.GetPostListByPageId<Lib.ViewModels.MixPagePosts.ReadViewModel>(
                pageId, keyword, culture, 
                orderBy, direction, pageSize, page -  1, _context, _transaction);
            result.Data.Items.ForEach(m => m.LoadPost(_context, _transaction));
            return result;
        }
        
        public static async Task<RepositoryResponse<PaginationModel<Lib.ViewModels.MixAttributeSetDatas.ReadMvcViewModel>>> GetAttributeDataListBySet(
            HttpContext context
            , string attributeSetName
            , string culture = null
            , Heart.Enums.MixHeartEnums.DisplayDirection direction = MixHeartEnums.DisplayDirection.Desc
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null
            )
        {            
            var result = await ViewModels.MixAttributeSetDatas.Helper.FilterByKeywordAsync<ViewModels.MixAttributeSetDatas.ReadMvcViewModel>(
                    context.Request, culture, attributeSetName);
            return result;
        }
    }
}