using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
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

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public ModuleType Type { get; set; }
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

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("columns")]
        public List<ModuleFieldViewModel> Columns
        {
            get { return Fields == null ? null : JsonConvert.DeserializeObject<List<ModuleFieldViewModel>>(Fields); }
            set { Fields = JsonConvert.SerializeObject(value); }
        }

        [JsonProperty("view")]
        public MixTemplates.ReadViewModel View { get; set; }

        [JsonProperty("data")]
        public PaginationModel<ViewModels.MixModuleDatas.ReadViewModel> Data { get; set; } = new PaginationModel<ViewModels.MixModuleDatas.ReadViewModel>();

        [JsonProperty("articles")]
        public PaginationModel<MixModuleArticles.ReadViewModel> Articles { get; set; } = new PaginationModel<MixModuleArticles.ReadViewModel>();

        [JsonProperty("products")]
        public PaginationModel<MixModuleProducts.ReadViewModel> Products { get; set; } = new PaginationModel<MixModuleProducts.ReadViewModel>();

        public string TemplatePath
        {
            get
            {
                return string.Format("../{0}", Template);
            }
        }

        #endregion Views

        public int ArticleId { get; set; }
        public int CategoryId { get; set; }

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
            this.View = MixTemplates.ReadViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;

            var getDataResult = MixModuleDatas.ReadViewModel.Repository
                .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , null, null
                , _context, _transaction);
            if (getDataResult.IsSucceed)
            {
                getDataResult.Data.JsonItems = new List<JObject>();
                getDataResult.Data.Items.ForEach(d => getDataResult.Data.JsonItems.Add(d.JItem));
                Data = getDataResult.Data;
            }

            var getArticles = MixModuleArticles.ReadViewModel.Repository.GetModelListBy(n => n.ModuleId == Id && n.Specificulture == Specificulture
            , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , 4, 0
                , _context: _context, _transaction: _transaction
                );
            if (getArticles.IsSucceed)
            {
                Articles = getArticles.Data;
            }

            var getProducts = MixModuleProducts.ReadViewModel.Repository.GetModelListBy(
                m => m.ModuleId == Id && m.Specificulture == Specificulture
            , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
            , null, null
                , _context: _context, _transaction: _transaction
                );
            if (getProducts.IsSucceed)
            {
                Products = getProducts.Data;
            }
        }

        #endregion Overrides

        #region Expand

        public static RepositoryResponse<ReadMvcViewModel> GetBy(
            Expression<Func<MixModule, bool>> predicate, int? articleId = null, int? productid = null, int categoryId = 0
             , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = Repository.GetSingleModel(predicate, _context, _transaction);
            if (result.IsSucceed)
            {
                result.Data.ArticleId = articleId.Value;
                result.Data.CategoryId = categoryId;
                result.Data.LoadData();
            }
            return result;
        }

        public void LoadData(int? articleId = null, int? categoryId = null
            , int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<PaginationModel<MixModuleDatas.ReadViewModel>> getDataResult = new RepositoryResponse<PaginationModel<MixModuleDatas.ReadViewModel>>();

            switch (Type)
            {
                case ModuleType.Root:
                    getDataResult = MixModuleDatas.ReadViewModel.Repository
                       .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                       , "Priority", 0, pageSize, pageIndex
                       , _context, _transaction);
                    break;

                case ModuleType.SubPage:
                    getDataResult = MixModuleDatas.ReadViewModel.Repository
                       .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                       && (m.CategoryId == categoryId)
                       , "Priority", 0, pageSize, pageIndex
                       , _context, _transaction);
                    break;

                case ModuleType.SubArticle:
                    getDataResult = MixModuleDatas.ReadViewModel.Repository
                       .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                       && (m.ArticleId == articleId)
                       , "Priority", 0, pageSize, pageIndex
                       , _context, _transaction);
                    break;

                default:
                    break;
            }

            if (getDataResult.IsSucceed)
            {
                getDataResult.Data.JsonItems = new List<JObject>();
                getDataResult.Data.Items.ForEach(d => getDataResult.Data.JsonItems.Add(d.JItem));
                Data = getDataResult.Data;
            }
        }

        #endregion Expand
    }
}
