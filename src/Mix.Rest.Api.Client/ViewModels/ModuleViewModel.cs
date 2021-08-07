using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Heart.NetCore.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mix.Rest.Api.Client.ViewModels
{
    [GeneratedController("api/v1/rest/{culture}/module/client")]
    public class ModuleViewModel
        : ViewModelBase<MixCmsContext, MixModule, ModuleViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("formTemplate")]
        public string FormTemplate { get; set; }

        [JsonProperty("edmTemplate")]
        public string EdmTemplate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public MixModuleType Type { get; set; }

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

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

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

        [JsonProperty("columns")]
        public List<ModuleFieldViewModel> Columns
        {
            get { return Fields == null ? null : JsonConvert.DeserializeObject<List<ModuleFieldViewModel>>(Fields); }
            set { Fields = JsonConvert.SerializeObject(value); }
        }

        [JsonProperty("data")]
        public PaginationModel<Cms.Lib.ViewModels.MixModuleDatas.ReadViewModel> Data { get; set; } = new PaginationModel<Cms.Lib.ViewModels.MixModuleDatas.ReadViewModel>();

        [JsonProperty("posts")]
        public PaginationModel<Cms.Lib.ViewModels.MixModulePosts.ReadViewModel> Posts { get; set; } = new PaginationModel<Cms.Lib.ViewModels.MixModulePosts.ReadViewModel>();

        [JsonProperty("additionalData")]
        public JObject AdditionalData { get; set; }

        #endregion Views

        public int? PostId { get; set; }
        public int? PageId { get; set; }

        #endregion Properties

        #region Contructors

        public ModuleViewModel() : base()
        {
        }

        public ModuleViewModel(MixModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            // call load data from controller for padding parameter (postId, productId, ...)
            LoadAttributes(_context, _transaction);
        }

        #endregion Overrides

        #region Expand

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var dataId = _context.MixDatabaseDataAssociation.Where(
                a => a.ParentId == Id.ToString()
                    && a.Specificulture == Specificulture
                    && a.MixDatabaseName == MixDatabaseNames.ADDITIONAL_COLUMN_MODULE)
                .Select(m => m.DataId)
                .FirstOrDefault();
            AdditionalData = Cms.Lib.ViewModels.MixDatabaseDatas.ReadMvcViewModel.Repository.GetFirstModel(
               a => a.Id == dataId && a.Specificulture == Specificulture
                   , _context, _transaction).Data?.Obj;
        }

        public static RepositoryResponse<ModuleViewModel> GetBy(
            Expression<Func<MixModule, bool>> predicate, int? postId = null, int? productid = null, int pageId = 0
             , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = Repository.GetSingleModel(predicate, _context, _transaction);
            if (result.IsSucceed)
            {
                result.Data.PostId = postId;
                result.Data.PageId = pageId;
                result.Data.LoadData();
            }
            return result;
        }

        public void LoadData(int? postId = null, int? productId = null, int? pageId = null
            , int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                pageSize = pageSize > 0 ? pageSize : PageSize;
                pageIndex = pageIndex > 0 ? pageIndex : 0;
                Expression<Func<MixModuleData, bool>> dataExp = null;
                Expression<Func<MixModulePost, bool>> postExp = null;
                switch (Type)
                {
                    case MixModuleType.Content:
                    case MixModuleType.Data:
                        dataExp = m => m.ModuleId == Id && m.Specificulture == Specificulture;
                        //postExp = n => n.ModuleId == Id && n.Specificulture == Specificulture;
                        //productExp = m => m.ModuleId == Id && m.Specificulture == Specificulture;
                        break;

                    case MixModuleType.ListPost:
                        postExp = n => n.ModuleId == Id && n.Specificulture == Specificulture;
                        break;

                    default:
                        dataExp = m => m.ModuleId == Id && m.Specificulture == Specificulture;
                        postExp = n => n.ModuleId == Id && n.Specificulture == Specificulture;
                        break;
                }

                if (dataExp != null)
                {
                    var getDataResult = Cms.Lib.ViewModels.MixModuleDatas.ReadViewModel.Repository
                    .GetModelListBy(
                        dataExp
                        , MixService.GetAppSetting<string>(MixAppSettingKeywords.SortBy
                        ), 0
                        , pageSize, pageIndex
                        , _context: context, _transaction: transaction);
                    if (getDataResult.IsSucceed)
                    {
                        //getDataResult.Data.JsonItems = new List<JObject>();
                        //getDataResult.Data.Items.ForEach(d => getDataResult.Data.JsonItems.Add(d.JItem));
                        Data = getDataResult.Data;
                    }
                }
                if (postExp != null)
                {
                    var getPosts = Cms.Lib.ViewModels.MixModulePosts.ReadViewModel.Repository
                    .GetModelListBy(postExp
                    , MixService.GetAppSetting<string>(MixAppSettingKeywords.SortBy), 0
                    , pageSize, pageIndex
                    , _context: context, _transaction: transaction);
                    if (getPosts.IsSucceed)
                    {
                        Posts = getPosts.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<ModuleViewModel>>(ex, isRoot, transaction);
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

        #endregion Expand
    }
}