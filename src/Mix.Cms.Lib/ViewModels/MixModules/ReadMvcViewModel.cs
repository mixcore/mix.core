using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixModules
{
    public class ReadMvcViewModel
        : ViewModelBase<MixCmsContext, MixModule, ReadMvcViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

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

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl {
            get {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Image
                });
                }
                else
                {
                    return Image;
                }
            }
        }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl {
            get {
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Thumbnail
                });
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        [JsonProperty("columns")]
        public List<ModuleFieldViewModel> Columns {
            get { return Fields == null ? null : JsonConvert.DeserializeObject<List<ModuleFieldViewModel>>(Fields); }
            set { Fields = JsonConvert.SerializeObject(value); }
        }

        [JsonProperty("view")]
        public MixTemplates.ReadListItemViewModel View { get; set; }

        [JsonProperty("formView")]
        public MixTemplates.ReadListItemViewModel FormView { get; set; }

        [JsonProperty("edmView")]
        public MixTemplates.ReadListItemViewModel EdmView { get; set; }

        [JsonProperty("data")]
        public PaginationModel<ViewModels.MixModuleDatas.ReadViewModel> Data { get; set; } = new PaginationModel<ViewModels.MixModuleDatas.ReadViewModel>();

        [JsonProperty("posts")]
        public PaginationModel<MixModulePosts.ReadViewModel> Posts { get; set; } = new PaginationModel<MixModulePosts.ReadViewModel>();

        public string TemplatePath {
            get {
                return CommonHelper.GetFullPath(new string[]
                {
                    ""
                    , MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, Specificulture) ?? "Default"
                    , Template
                });
            }
        }

        public string FormTemplatePath {
            get {
                return CommonHelper.GetFullPath(new string[]
                {
                    ""
                    , MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, Specificulture) ?? "Default"
                    , FormTemplate
                });
            }
        }

        public string EdmTemplatePath {
            get {
                return CommonHelper.GetFullPath(new string[]
                {
                    ""
                    , MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, Specificulture) ?? "Default"
                    , EdmTemplate
                });
            }
        }

        [JsonProperty("attributeData")]
        public MixRelatedAttributeDatas.ReadMvcViewModel AttributeData { get; set; }

        #endregion Views

        public int? PostId { get; set; }
        public int? PageId { get; set; }

        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //Load Template + Style +  Scripts for views
            this.View = MixTemplates.ReadListItemViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;
            this.FormView = MixTemplates.ReadListItemViewModel.GetTemplateByPath(FormTemplate, Specificulture, _context, _transaction).Data;
            this.EdmView = MixTemplates.ReadListItemViewModel.GetTemplateByPath(EdmTemplate, Specificulture, _context, _transaction).Data;
            LoadAttributes(_context, _transaction);
            // call load data from controller for padding parameter (postId, productId, ...)
        }

        #endregion Overrides

        #region Expand

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getAttrs = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(m => m.Name == MixConstants.AttributeSetName.ADDITIONAL_FIELD_MODULE, _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                AttributeData = MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.AttributeSetId == getAttrs.Data.Id
                    , _context, _transaction).Data;
            }
        }

        public static RepositoryResponse<ReadMvcViewModel> GetBy(
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

                    case MixModuleType.SubPage:
                        dataExp = m => m.ModuleId == Id && m.Specificulture == Specificulture && (m.PageId == pageId);
                        postExp = n => n.ModuleId == Id && n.Specificulture == Specificulture;
                        break;

                    case MixModuleType.SubPost:
                        dataExp = m => m.ModuleId == Id && m.Specificulture == Specificulture && (m.PostId == postId);
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
                    var getDataResult = MixModuleDatas.ReadViewModel.Repository
                    .GetModelListBy(
                        dataExp
                        , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                        , pageSize, pageIndex
                        , _context: context, _transaction: transaction);
                    if (getDataResult.IsSucceed)
                    {
                        getDataResult.Data.JsonItems = new List<JObject>();
                        getDataResult.Data.Items.ForEach(d => getDataResult.Data.JsonItems.Add(d.JItem));
                        Data = getDataResult.Data;
                    }
                }
                if (postExp != null)
                {
                    var getPosts = MixModulePosts.ReadViewModel.Repository
                    .GetModelListBy(postExp
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
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
                UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<ReadMvcViewModel>>(ex, isRoot, transaction);
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

        public T Property<T>(string fieldName)
        {
            if (AttributeData != null)
            {
                var field = AttributeData.Data.Data.GetValue(fieldName);
                if (field != null)
                {
                    return field.Value<T>();
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        #endregion Expand
    }
}