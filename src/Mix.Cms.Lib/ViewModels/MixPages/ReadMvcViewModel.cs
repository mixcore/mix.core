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
using System.Text;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPages
{
    public class ReadMvcViewModel: ViewModelBase<MixCmsContext, MixPage, ReadMvcViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public MixEnums.MixPageType Type { get; set; }

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("cssClass")]
        public string CssClass { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("staticUrl")]
        public string StaticUrl { get; set; }

        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("views")]
        public int? Views { get; set; }

        [JsonProperty("seoName")]
        public string SeoName { get; set; }

        [JsonProperty("seoTitle")]
        public string SeoTitle { get; set; }

        [JsonProperty("seoDescription")]
        public string SeoDescription { get; set; }

        [JsonProperty("seoKeywords")]
        public string SeoKeywords { get; set; }

        [JsonProperty("level")]
        public int? Level { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("updatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("details")]
        public string DetailsUrl { get; set; }

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

        [JsonProperty("view")]
        public MixTemplates.ReadViewModel View { get; set; }

        [JsonProperty("articles")]
        public PaginationModel<MixPageArticles.ReadViewModel> Articles { get; set; } = new PaginationModel<MixPageArticles.ReadViewModel>();

        [JsonProperty("products")]
        public PaginationModel<MixPageProducts.ReadViewModel> Products { get; set; } = new PaginationModel<MixPageProducts.ReadViewModel>();

        [JsonProperty("modules")]
        public List<MixPageModules.ReadMvcViewModel> Modules { get; set; } = new List<MixPageModules.ReadMvcViewModel>(); // Get All Module

        public string TemplatePath
        {
            get
            {
                return CommonHelper.GetFullPath(new string[]
                {
                    ""
                    , MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, Specificulture)??  MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultTemplateFolder)
                    , Template
                });
            }
        }

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
            this.View = MixTemplates.ReadViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;
            if (View != null)
            {
                switch (Type)
                {
                    case MixPageType.Home:
                        GetSubModules(_context, _transaction);
                        break;

                    case MixPageType.Blank:
                        break;

                    case MixPageType.Article:
                        break;

                    case MixPageType.Modules:
                        GetSubModules(_context, _transaction);
                        break;

                    case MixPageType.ListArticle:
                        GetSubArticles(_context, _transaction);
                        break;

                    case MixPageType.ListProduct:
                        GetSubProducts(_context, _transaction);
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion Overrides

        #region Expands

        #region Sync

        private void GetSubModules(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getNavs = MixPageModules.ReadMvcViewModel.Repository.GetModelListBy(
                m => m.CategoryId == Id && m.Specificulture == Specificulture
                , _context, _transaction);
            if (getNavs.IsSucceed)
            {
                Modules = getNavs.Data;
                StringBuilder scripts = new StringBuilder();
                StringBuilder styles = new StringBuilder();
                foreach (var nav in getNavs.Data.OrderBy(n => n.Priority).ToList())
                {
                    scripts.Append(nav.Module.View.Scripts);
                    styles.Append(nav.Module.View.Styles);
                }
                View.Scripts = scripts.ToString();
                View.Styles = styles.ToString();
            }
        }

        private void GetSubArticles(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getArticles = MixPageArticles.ReadViewModel.Repository.GetModelListBy(
                n => n.CategoryId == Id && n.Specificulture == Specificulture,
                MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , 4, 0
               , _context: _context, _transaction: _transaction
               );
            if (getArticles.IsSucceed)
            {
                Articles = getArticles.Data;
            }
        }

        private void GetSubProducts(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getProducts = MixPageProducts.ReadViewModel.Repository.GetModelListBy(
               m => m.CategoryId == Id && m.Specificulture == Specificulture
           , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
           , PageSize, 0
               , _context: _context, _transaction: _transaction
               );
            if (getProducts.IsSucceed)
            {
                Products = getProducts.Data;
            }
        }

        #endregion Sync

        public MixModules.ReadMvcViewModel GetModule(string name)
        {
            return Modules.FirstOrDefault(m => m.Module.Name == name)?.Module;
        }

        #endregion Expands
    }
}
