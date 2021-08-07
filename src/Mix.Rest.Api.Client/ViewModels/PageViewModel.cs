using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
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
using MixLibViewModels = Mix.Cms.Lib.ViewModels;

namespace Mix.Rest.Api.Client.ViewModels
{
    public class PageViewModel : ViewModelBase<MixCmsContext, MixPage, PageViewModel>
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
        public string DetailsUrl { get => Id > 0 ? $"/{Specificulture}/page/{SeoName}" : null; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return $"{Domain}/{Image}";
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
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return $"{Domain}/{Thumbnail}";
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        [JsonProperty("posts")]
        public PaginationModel<PostViewModel> Posts { get; set; } = new PaginationModel<PostViewModel>();

        [JsonProperty("modules")]
        public List<ModuleViewModel> Modules { get; set; } = new List<ModuleViewModel>(); // Get All Module

        public string TemplatePath
        {
            get
            {
                return $"/{MixFolders.TemplatesFolder}/{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture)}/{Template}";
            }
        }

        [JsonProperty("additionalData")]
        public JObject AdditionalData { get; set; }

        [JsonProperty("bodyClass")]
        public string BodyClass => CssClass;

        #endregion Views

        #endregion Properties

        #region Contructors

        public PageViewModel() : base()
        {
        }

        public PageViewModel(MixPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            GetSubModules(_context, _transaction);
            LoadAttributes(_context, _transaction);
        }

        #endregion Overrides

        #region Expands

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
                    item.LoadData(_context: context, _transaction: transaction);
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
                    var postIds = context.MixPagePost.Where(postExp).Select(m => m.PostId);
                    Posts = PostViewModel.Repository.GetModelListBy(
                        m => m.Specificulture == Specificulture && postIds.Any(n => n == m.Id),
                        MixService.GetAppSetting<string>(MixAppSettingKeywords.SortBy),
                        0,
                        pageSize,
                        pageIndex
                    , _context: context, _transaction: transaction
                        ).Data;
                }
            }
            catch (Exception ex)
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<PageViewModel>>(ex, isRoot, transaction);
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
            var moduleIds = _context.MixPageModule.Where(
                m => m.PageId == Id && m.Specificulture == Specificulture
                ).Select(m => m.ModuleId);
            Modules = ModuleViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture
             && moduleIds.Any(n => n == m.Id), _context, _transaction).Data;

        }

        #endregion Sync

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var dataId = _context.MixDatabaseDataAssociation.Where(
                a => a.ParentId == Id.ToString()
                    && a.Specificulture == Specificulture
                    && a.MixDatabaseName == MixDatabaseNames.ADDITIONAL_COLUMN_PAGE)
                .Select(m => m.DataId)
                .FirstOrDefault();
            AdditionalData = MixLibViewModels.MixDatabaseDatas.ReadMvcViewModel.Repository.GetFirstModel(
               a => a.Id == dataId && a.Specificulture == Specificulture
                   , _context, _transaction).Data?.Obj;
        }
        #endregion Expands
    }
}