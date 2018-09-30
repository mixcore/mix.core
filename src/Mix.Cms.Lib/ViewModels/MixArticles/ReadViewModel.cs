using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixArticles
{
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixArticle, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty("seoName")]
        public string SeoName { get; set; }

        [JsonProperty("views")]
        public int? Views { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain", Specificulture) ?? "/"; } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (Image != null && (Image.IndexOf("http") == -1 && Image[0] != '/'))
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

        public string ThumbnailUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Thumbnail))
                {
                    if (Thumbnail.IndexOf("http") == -1)
                    {
                        return CommonHelper.GetFullPath(new string[] {
                            Domain,  Thumbnail

                        });
                    }
                    else
                    {
                        return Thumbnail;
                    }

                }
                else
                {
                    return ImageUrl;
                }
            }
        }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixArticle model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Expands

        public static async Task<RepositoryResponse<PaginationModel<ReadViewModel>>> GetModelListByCategoryAsync(
            int categoryId, string specificulture
            , string orderByPropertyName, int direction
            , int? pageSize = 1, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            MixCmsContext context = _context ?? new MixCmsContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var query = context.MixPageArticle.Include(ac => ac.MixArticle)
                    .Where(ac =>
                    ac.CategoryId == categoryId && ac.Specificulture == specificulture
                    && ac.Status == (int)MixEnums.MixContentStatus.Published).Select(ac => ac.MixArticle);
                PaginationModel<ReadViewModel> result = await Repository.ParsePagingQueryAsync(
                    query, orderByPropertyName
                    , direction,
                    pageSize, pageIndex, context, transaction
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
                if (_transaction == null)
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
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        #region Sync

        public static RepositoryResponse<PaginationModel<ReadViewModel>> GetModelListByCategory(
           int categoryId, string specificulture
           , string orderByPropertyName, int direction
           , int? pageSize = 1, int? pageIndex = 0
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            MixCmsContext context = _context ?? new MixCmsContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var query = context.MixPageArticle.Include(ac => ac.MixArticle)
                    .Where(ac =>
                    ac.CategoryId == categoryId && ac.Specificulture == specificulture
                    && ac.Status == (int)MixContentStatus.Published).Select(ac => ac.MixArticle);
                PaginationModel<ReadViewModel> result = Repository.ParsePagingQuery(
                    query, orderByPropertyName
                    , direction,
                    pageSize, pageIndex, context, transaction
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
                if (_transaction == null)
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
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        public static RepositoryResponse<PaginationModel<ReadViewModel>> GetModelListByModule(
          int ModuleId, string specificulture
          , string orderByPropertyName, int direction
          , int? pageSize = 1, int? pageIndex = 0
          , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            MixCmsContext context = _context ?? new MixCmsContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var query = context.MixModuleArticle.Include(ac => ac.MixArticle)
                    .Where(ac =>
                    ac.ModuleId == ModuleId && ac.Specificulture == specificulture
                    && (ac.Status == (int)MixContentStatus.Published || ac.Status == (int)MixContentStatus.Preview)).Select(ac => ac.MixArticle);
                PaginationModel<ReadViewModel> result = Repository.ParsePagingQuery(
                    query, orderByPropertyName
                    , direction,
                    pageSize, pageIndex, context, transaction
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
                if (_transaction == null)
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
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        #endregion Sync

        #endregion Expands

        #region Overrides

        #endregion
    }
}
