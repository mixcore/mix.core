using Microsoft.EntityFrameworkCore;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixPost, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("cultures")]
        public List<Domain.Core.Models.SupportedCulture> Cultures { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonIgnore]
        [JsonProperty("extraFields")]
        public string ExtraFields { get; set; } = "[]";

        [JsonIgnore]
        [JsonProperty("extraProperties")]
        public string ExtraProperties { get; set; } = "[]";

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [Required]
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
        public string Type { get; set; }

        [JsonProperty("publishedDateTime")]
        public DateTime? PublishedDateTime { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; } = "[]";

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
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

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

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get => Id > 0 ? $"/post/{Specificulture}/{Id}/{SeoName}" : null; }

        [JsonProperty("properties")]
        public List<ExtraProperty> Properties { get; set; }

        [JsonProperty("attributeData")]
        public MixRelatedAttributeDatas.ReadMvcViewModel AttributeData { get; set; }

        [JsonProperty("sysTags")]
        public List<MixRelatedAttributeDatas.FormViewModel> SysTags { get; set; } = new List<MixRelatedAttributeDatas.FormViewModel>();

        [JsonProperty("sysCategories")]
        public List<MixRelatedAttributeDatas.FormViewModel> SysCategories { get; set; } = new List<MixRelatedAttributeDatas.FormViewModel>();

        [JsonProperty("listTag")]
        public List<string> ListTag { get => SysTags.Select(t => t.AttributeData.Property<string>("title")).Distinct().ToList(); }

        [JsonProperty("listCategory")]
        public List<string> ListCategory { get => SysCategories.Select(t => t.AttributeData.Property<string>("title")).Distinct().ToList(); }
        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Expands

        //Get Property by name
        public string Property(string name)
        {
            var prop = Properties.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            return prop?.Value;
        }

        public static async Task<RepositoryResponse<PaginationModel<ReadViewModel>>> GetModelListByCategoryAsync(
            int pageId, string specificulture
            , string orderByPropertyName, Heart.Enums.MixHeartEnums.DisplayDirection direction
            , int? pageSize = 1, int? pageIndex = 0, int? skip = null, int? top = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var query = context.MixPagePost.Include(ac => ac.MixPost)
                    .Where(ac =>
                    ac.PageId == pageId && ac.Specificulture == specificulture
                    && ac.Status == MixEnums.MixContentStatus.Published).Select(ac => ac.MixPost);
                PaginationModel<ReadViewModel> result = await Repository.ParsePagingQueryAsync(
                    query, orderByPropertyName
                    , direction
                    , pageSize, pageIndex, skip, top
                    , context, transaction
                    );
                return new RepositoryResponse<PaginationModel<ReadViewModel>>()
                {
                    IsSucceed = true,
                    Data = result
                };
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                Repository.LogErrorMessage(ex);
                if (isRoot)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<PaginationModel<ReadViewModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
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

        #region Sync

        public static RepositoryResponse<PaginationModel<ReadViewModel>> GetModelListByCategory(
           int pageId, string specificulture
           , string orderByPropertyName, Heart.Enums.MixHeartEnums.DisplayDirection direction
           , int? pageSize = 1, int? pageIndex = 0
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var query = context.MixPagePost.Include(ac => ac.MixPost)
                    .Where(ac =>
                    ac.PageId == pageId && ac.Specificulture == specificulture
                    && ac.Status == MixContentStatus.Published).Select(ac => ac.MixPost);
                PaginationModel<ReadViewModel> result = Repository.ParsePagingQuery(
                    query, orderByPropertyName
                    , direction
                    , pageSize, pageIndex
                    , context, transaction
                    );
                return new RepositoryResponse<PaginationModel<ReadViewModel>>()
                {
                    IsSucceed = true,
                    Data = result
                };
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                Repository.LogErrorMessage(ex);
                if (isRoot)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<PaginationModel<ReadViewModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
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

        public static RepositoryResponse<PaginationModel<ReadViewModel>> GetModelListByModule(
          int ModuleId, string specificulture
          , string orderByPropertyName, Heart.Enums.MixHeartEnums.DisplayDirection direction
          , int? pageSize = 1, int? pageIndex = 0
          , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var query = context.MixModulePost.Include(ac => ac.MixPost)
                    .Where(ac =>
                    ac.ModuleId == ModuleId && ac.Specificulture == specificulture
                    && (ac.Status == MixContentStatus.Published || ac.Status == MixContentStatus.Preview)).Select(ac => ac.MixPost);
                PaginationModel<ReadViewModel> result = Repository.ParsePagingQuery(
                    query, orderByPropertyName
                    , direction
                    , pageSize, pageIndex
                    , context, transaction
                    );
                return new RepositoryResponse<PaginationModel<ReadViewModel>>()
                {
                    IsSucceed = true,
                    Data = result
                };
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                Repository.LogErrorMessage(ex);
                if (isRoot)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<PaginationModel<ReadViewModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
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

        #endregion Sync

        #endregion Expands

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            LoadAttributes(_context, _transaction);
            LoadTags(_context, _transaction);
            LoadCategories(_context, _transaction);
        }

        private void LoadTags(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getTags = MixRelatedAttributeDatas.FormViewModel.Repository.GetModelListBy(
                    m => m.Specificulture == Specificulture && m.Status == MixEnums.MixContentStatus.Published
                   && m.ParentId == Id.ToString() && m.ParentType == MixEnums.MixAttributeSetDataType.Post.ToString()
                   && m.AttributeSetName == MixConstants.AttributeSetName.SYSTEM_TAG, context, transaction);
            if (getTags.IsSucceed)
            {
                SysTags = getTags.Data;
            }
        }

        private void LoadCategories(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getData = MixRelatedAttributeDatas.FormViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture
                   && m.ParentId == Id.ToString() && m.ParentType == MixEnums.MixAttributeSetDataType.Post.ToString()
                   && m.AttributeSetName == MixConstants.AttributeSetName.SYSTEM_CATEGORY, context, transaction);
            if (getData.IsSucceed)
            {
                SysCategories = getData.Data;
            }
        }

        #endregion Overrides

        #region Expands

        /// <summary>Loads the attributes.</summary>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getAttrs = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(m => m.Name == MixConstants.AttributeSetName.ADDITIONAL_FIELD_POST, _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                AttributeData = MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.AttributeSetId == getAttrs.Data.Id
                    , _context, _transaction).Data;
            }
        }

        /// <summary>Get Post's Property by type and name</summary>
        /// <typeparam name="T">Type of field</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>T</returns>
        public T Property<T>(string fieldName)
        {
            if (AttributeData != null)
            {
                var field = AttributeData.Data.Obj.GetValue(fieldName);
                if (field != null)
                {
                    return field.Value<T>();
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }
        #endregion Overrides
    }
}