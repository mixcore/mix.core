using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Interfaces;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mix.Cms.Lib.ViewModels.MixPages
{
    public class ReadMvcViewModel : ViewModelBase<MixCmsContext, MixPage, ReadMvcViewModel>, IMvcViewModel
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("cssClass")]
        public string CssClass { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("seoName")]
        public string SeoName { get; set; }

        [JsonProperty("seoTitle")]
        public string SeoTitle { get; set; }

        [JsonProperty("seoDescription")]
        public string SeoDescription { get; set; }

        [JsonProperty("seoKeywords")]
        public string SeoKeywords { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("views")]
        public int? Views { get; set; }

        [JsonProperty("type")]
        public MixPageType Type { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("staticUrl")]
        public string StaticUrl { get; set; }

        [JsonProperty("level")]
        public int? Level { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("details")]
        public string DetailsUrl { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1))
                {
                    return $"{Domain.TrimEnd('/')}/{Image.TrimStart('/')}";
                }
                else
                {
                    return Image;
                }
            }
        }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl
        {
            get
            {
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1)
                {
                    return $"{Domain.TrimEnd('/')}/{Thumbnail.TrimStart('/')}";
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        [JsonProperty("view")]
        public MixTemplates.ReadListItemViewModel View { get; set; }

        [JsonProperty("posts")]
        public PaginationModel<MixPagePosts.ReadViewModel> Posts { get; set; } = new PaginationModel<MixPagePosts.ReadViewModel>();

        [JsonProperty("modules")]
        public List<MixPageModules.ReadMvcViewModel> Modules { get; set; } = new List<MixPageModules.ReadMvcViewModel>(); // Get All Module

        public string TemplatePath
        {
            get
            {
                return $"/{MixFolders.TemplatesFolder}/{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture)}/{Template}";
            }
        }

        [JsonProperty("additionalData")]
        public MixDatabaseDataAssociations.ReadMvcViewModel AdditionalData { get; set; }

        [JsonProperty("bodyClass")]
        public string BodyClass => CssClass;

        [JsonProperty("urlAliases")]
        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.View = MixTemplates.ReadListItemViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;
            if (View != null)
            {
                GetSubModules(_context, _transaction);
            }
            LoadAttributes(_context, _transaction);
            UrlAliases = GetAliases(_context, _transaction);
            DetailsUrl = UrlAliases.Count > 0
              ? MixCmsHelper.GetDetailsUrl(Specificulture, $"/{UrlAliases[0].Alias}")
              : Id > 0
                  ? MixCmsHelper.GetDetailsUrl(Specificulture, $"/{SeoName}")
                  : null;
        }

        #endregion Overrides

        #region Expands
        
        public List<MixUrlAliases.UpdateViewModel> GetAliases(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixUrlAliases.UpdateViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture
                        && p.SourceId == Id.ToString() && p.Type == (int)MixUrlAliasType.Page, context, transaction);
            if (result.IsSucceed && result.Data != null)
            {
                return result.Data;
            }
            else
            {
                return new List<MixUrlAliases.UpdateViewModel>();
            }
        }

        #region Sync

        public void LoadData(int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                pageSize = pageSize > 0 ? pageSize : PageSize;
                pageIndex = pageIndex > 0 ? pageIndex : 0;
                Expression<Func<MixPageModule, bool>> dataExp = null;
                Expression<Func<MixPagePost, bool>> postExp = null;
                foreach (var item in Modules)
                {
                    item.Module.LoadData(_context: context, _transaction: transaction, pageIndex: pageIndex);
                }
                switch (Type)
                {
                    case MixPageType.ListPost:
                        postExp = n => n.PageId == Id && n.Specificulture == Specificulture;
                        break;

                    default:
                        dataExp = m => m.PageId == Id && m.Specificulture == Specificulture;
                        postExp = n => n.PageId == Id && n.Specificulture == Specificulture;
                        break;
                }

                if (postExp != null)
                {
                    var getPosts = MixPagePosts.ReadViewModel.Repository
                    .GetModelListBy(postExp
                    , MixService.GetAppSetting<string>(MixAppSettingKeywords.SortBy), 0
                    , pageSize, pageIndex
                    , _context: context, _transaction: transaction);
                    if (getPosts.IsSucceed)
                    {
                        Posts = getPosts.Data;
                        Posts.Items.ForEach(m => m.LoadPost(context, transaction));
                    }
                }
            }
            catch (Exception ex)
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<ReadMvcViewModel>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public void LoadDataByTag(string tagName
            , string orderBy, int orderDirection
            , int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                pageSize = pageSize > 0 ? pageSize : PageSize;
                pageIndex = pageIndex ?? 0;
                Expression<Func<MixPost, bool>> postExp = null;
                JObject obj = new JObject(new JProperty("text", tagName));

                postExp = n => n.Tags.Contains(obj.ToString(Newtonsoft.Json.Formatting.None)) && n.Specificulture == Specificulture;

                if (postExp != null)
                {
                    var getPosts = MixPosts.ReadListItemViewModel.Repository
                    .GetModelListBy(postExp
                    , MixService.GetAppSetting<string>(orderBy), 0
                    , pageSize, pageIndex
                    , _context: context, _transaction: transaction);
                    if (getPosts.IsSucceed)
                    {
                        Posts.Items = new List<MixPagePosts.ReadViewModel>();
                        Posts.PageIndex = getPosts.Data.PageIndex;
                        Posts.PageSize = getPosts.Data.PageSize;
                        Posts.TotalItems = getPosts.Data.TotalItems;
                        Posts.TotalPage = getPosts.Data.TotalPage;
                        foreach (var post in getPosts.Data.Items)
                        {
                            Posts.Items.Add(new MixPagePosts.ReadViewModel()
                            {
                                PageId = Id,
                                PostId = post.Id,
                                Post = post
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<ReadMvcViewModel>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public void LoadDataByKeyword(string keyword
           , string orderBy, int orderDirection
           , int? pageSize = null, int? pageIndex = null
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                pageSize = pageSize > 0 ? pageSize : PageSize;
                pageIndex = pageIndex ?? 0;
                Expression<Func<MixPost, bool>> postExp = null;

                postExp = n => n.Title.Contains(keyword) && n.Specificulture == Specificulture;

                if (postExp != null)
                {
                    var getPosts = MixPosts.ReadListItemViewModel.Repository
                    .GetModelListBy(postExp
                    , MixService.GetAppSetting<string>(orderBy), 0
                    , pageSize, pageIndex
                    , _context: context, _transaction: transaction);
                    if (getPosts.IsSucceed)
                    {
                        Posts.Items = new List<MixPagePosts.ReadViewModel>();
                        Posts.PageIndex = getPosts.Data.PageIndex;
                        Posts.PageSize = getPosts.Data.PageSize;
                        Posts.TotalItems = getPosts.Data.TotalItems;
                        Posts.TotalPage = getPosts.Data.TotalPage;
                        foreach (var post in getPosts.Data.Items)
                        {
                            Posts.Items.Add(new MixPagePosts.ReadViewModel()
                            {
                                PageId = Id,
                                PostId = post.Id,
                                Post = post
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<ReadMvcViewModel>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        private void GetSubModules(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getNavs = MixPageModules.ReadMvcViewModel.Repository.GetModelListBy(
                m => m.PageId == Id && m.Specificulture == Specificulture
                , _context, _transaction);
            if (getNavs.IsSucceed)
            {
                Modules = getNavs.Data;
                StringBuilder scripts = new StringBuilder();
                StringBuilder styles = new StringBuilder();
                foreach (var nav in getNavs.Data.OrderBy(n => n.Priority).ToList())
                {
                    string script = $"<!-- Start script module {nav.Module.Name} --> {nav.Module.View?.Scripts} <!-- End script module {nav.Module.Name} -->";
                    string style = $"<!-- Start style module {nav.Module.Name} --> {nav.Module.View?.Styles} <!-- End style module {nav.Module.Name} -->";
                    scripts.Append(script);
                    styles.Append(style);
                }
                View.Scripts += scripts.ToString();
                View.Styles += styles.ToString();
            }
        }

        private void GetSubPosts(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPosts = MixPagePosts.ReadViewModel.Repository.GetModelListBy(
                n => n.PageId == Id && n.Specificulture == Specificulture,
                MixService.GetAppSetting<string>(MixAppSettingKeywords.SortBy), 0
                , 4, 0
               , _context: _context, _transaction: _transaction
               );
            if (getPosts.IsSucceed)
            {
                Posts = getPosts.Data;
            }
        }

        #endregion Sync

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getAttrs = MixDatabases.UpdateViewModel.Repository.GetSingleModel(m => m.Name == MixConstants.MixDatabaseName.ADDITIONAL_COLUMN_PAGE, _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                AdditionalData = MixDatabaseDataAssociations.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.MixDatabaseId == getAttrs.Data.Id
                    , _context, _transaction).Data;
            }
        }

        public MixModules.ReadMvcViewModel GetModule(string name)
        {
            return Modules.FirstOrDefault(m => m.Module.Name == name)?.Module;
        }

        public bool HasValue(string fieldName)
        {
            return AdditionalData != null && AdditionalData.Data.Obj.GetValue(fieldName) != null;
        }

        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(AdditionalData?.Data?.Obj, fieldName);
        }

        #endregion Expands
    }
}