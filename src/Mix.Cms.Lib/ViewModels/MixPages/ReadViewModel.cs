using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPages
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPage, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public MixPageType Type { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }


        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("cssClass")]
        public string CssClass { get; set; }

        [JsonProperty("staticUrl")]
        public string StaticUrl { get; set; }

        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("seoName")]
        public string SeoName { get; set; }

        [JsonProperty("seoDescription")]
        public string SeoDescription { get; set; }

        [JsonProperty("seoKeywords")]
        public string SeoKeywords { get; set; }

        [JsonProperty("seoTitle")]
        public string SeoTitle { get; set; }

        [JsonProperty("level")]
        public int? Level { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain") ?? "/"; } }
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

        [JsonProperty("childs")]
        public List<ReadViewModel> Childs { get; set; }

        [JsonProperty("totalArticle")]
        public int TotalArticle { get; set; }

        [JsonProperty("totalProduct")]
        public int TotalProduct { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getChilds = Repository.GetModelListBy
                (p => p.MixPagePageMixPage.Any(c => c.ParentId == Id
                && c.Specificulture == Specificulture)
                );
            if (getChilds.IsSucceed)
            {
                Childs = getChilds.Data;
            }
            var countArticle = MixPageArticles.ReadViewModel.Repository.Count(c => c.CategoryId == Id && c.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction);

            if (countArticle.IsSucceed)
            {
                TotalArticle = countArticle.Data;
            }

            var countProduct = MixPageProducts.ReadViewModel.Repository.Count(c => c.CategoryId == Id && c.Specificulture == Specificulture
               , _context: _context, _transaction: _transaction);

            if (countProduct.IsSucceed)
            {
                TotalProduct = countProduct.Data;
            }
        }

        #endregion Overrides

        #region Expands

        public static async Task<RepositoryResponse<List<ReadViewModel>>> UpdateInfosAsync(List<ReadViewModel> cates)
        {
            MixCmsContext context = new MixCmsContext();
            var transaction = context.Database.BeginTransaction();
            var result = new RepositoryResponse<List<ReadViewModel>>();
            try
            {

                foreach (var item in cates)
                {
                    item.LastModified = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Errors.AddRange(saveResult.Errors);
                        result.Exception = saveResult.Exception;
                        break;
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, true, transaction);
                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<ReadViewModel>(ex, true, transaction);
                return new RepositoryResponse<List<ReadViewModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                //if current Context is Root
                transaction.Dispose();
                context.Dispose();
            }
        }
        #endregion
    }
}
