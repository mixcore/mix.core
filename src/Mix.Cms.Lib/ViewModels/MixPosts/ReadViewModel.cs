using Microsoft.EntityFrameworkCore;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public List<SupportedCulture> Cultures { get; set; }

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
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
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
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return $"{Domain.TrimEnd('/')}/{Thumbnail.TrimStart('/')}";
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get => Id > 0 ? $"/{Specificulture}/{MixController.Post}/{Id}/{SeoName}" : null; }

        [JsonProperty("properties")]
        public List<ExtraProperty> Properties { get; set; }

        [JsonProperty("additionalData")]
        public MixDatabaseDataAssociations.ReadMvcViewModel AdditionalData { get; set; }

        [JsonProperty("sysTags")]
        public List<MixDatabaseDataAssociations.FormViewModel> SysTags { get; set; } = new List<MixDatabaseDataAssociations.FormViewModel>();

        [JsonProperty("sysCategories")]
        public List<MixDatabaseDataAssociations.FormViewModel> SysCategories { get; set; } = new List<MixDatabaseDataAssociations.FormViewModel>();

        [JsonProperty("listTag")]
        public List<string> ListTag { get => SysTags.Select(t => t.AttributeData?.Property<string>("title")).Distinct().ToList(); }

        [JsonProperty("listCategory")]
        public List<string> ListCategory { get => SysCategories.Select(t => t.AttributeData?.Property<string>("title")).Distinct().ToList(); }

        [JsonProperty("author")]
        public JObject Author { get; set; }
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
            , string orderByPropertyName, Heart.Enums.DisplayDirection direction
            , int? pageSize = 1, int? pageIndex = 0, int? skip = null, int? top = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var query = context.MixPagePost.Include(ac => ac.MixPost)
                    .Where(ac =>
                    ac.PageId == pageId && ac.Specificulture == specificulture
                    && ac.Status == MixContentStatus.Published).Select(ac => ac.MixPost);
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
           , string orderByPropertyName, Heart.Enums.DisplayDirection direction
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
                    , null, null
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
          , string orderByPropertyName, Heart.Enums.DisplayDirection direction
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
                    , null, null
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
            LoadAuthor(_context, _transaction);
        }

        private void LoadTags(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getTags = MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(
                    m => m.Specificulture == Specificulture && m.Status == MixContentStatus.Published
                   && m.ParentId == Id.ToString() && m.ParentType == MixDatabaseParentType.Post
                   && m.MixDatabaseName == MixConstants.MixDatabaseName.SYSTEM_TAG, context, transaction);
            if (getTags.IsSucceed)
            {
                SysTags = getTags.Data;
            }
        }

        private void LoadAuthor(MixCmsContext context, IDbContextTransaction transaction)
        {
            if (!string.IsNullOrEmpty(CreatedBy))
            {
                var getAuthor = MixDatabaseDatas.Helper.LoadAdditionalData(MixDatabaseParentType.User, CreatedBy, MixDatabaseNames.SYSTEM_USER_DATA
                    , Specificulture, context, transaction);
                if (getAuthor.IsSucceed)
                {
                    Author = getAuthor.Data.Obj;
                }
            }
        }

        private void LoadCategories(MixCmsContext context, IDbContextTransaction transaction)
        {
            var getData = MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture
                   && m.ParentId == Id.ToString() && m.ParentType == MixDatabaseParentType.Post
                   && m.MixDatabaseName == MixConstants.MixDatabaseName.SYSTEM_CATEGORY, context, transaction);
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
            var getAttrs = MixDatabases.UpdateViewModel.Repository.GetSingleModel(m => m.Name == MixConstants.MixDatabaseName.ADDITIONAL_COLUMN_POST, _context, _transaction);
            if (getAttrs.IsSucceed)
            {
                AdditionalData = MixDatabaseDataAssociations.ReadMvcViewModel.Repository.GetFirstModel(
                a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.MixDatabaseId == getAttrs.Data.Id
                    , _context, _transaction).Data;
            }
        }

        /// <summary>Get Post's Property by type and name</summary>
        /// <typeparam name="T">Type of field</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>T</returns>
        public T Property<T>(string fieldName)
        {
            if (AdditionalData != null)
            {
                var field = AdditionalData.Data.Obj.GetValue(fieldName);
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

        #endregion Expands
    }
}