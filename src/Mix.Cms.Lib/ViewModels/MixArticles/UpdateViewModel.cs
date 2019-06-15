using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixSystem;
using Mix.Common.Helper;
using Mix.Domain.Core.Models;
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

namespace Mix.Cms.Lib.ViewModels.MixArticles
{
    public class UpdateViewModel
         : ViewModelBase<MixCmsContext, MixArticle, UpdateViewModel>
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
        public int Type { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("publishedDateTime")]
        public DateTime? PublishedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; } = "[]";

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain => MixService.GetConfig<string>("Domain");

        [JsonProperty("categories")]
        public List<MixPageArticles.ReadViewModel> Pages { get; set; }

        [JsonProperty("modules")]
        public List<MixModuleArticles.ReadViewModel> Modules { get; set; } // Parent to Modules

        [JsonProperty("mediaNavs")]
        public List<MixArticleMedias.ReadViewModel> MediaNavs { get; set; }

        [JsonProperty("moduleNavs")]
        public List<MixArticleModules.ReadViewModel> ModuleNavs { get; set; }

        [JsonProperty("articleNavs")]
        public List<MixArticleArticles.ReadViewModel> ArticleNavs { get; set; }

        [JsonProperty("attributeSetNavs")]
        public List<MixArticleAttributeSets.UpdateViewModel> AttributeSetNavs { get; set; }

        [JsonProperty("listTag")]
        public JArray ListTag { get; set; } = new JArray();

        [JsonProperty("imageFileStream")]
        public FileStreamViewModel ImageFileStream { get; set; }

        [JsonProperty("thumbnailFileStream")]
        public FileStreamViewModel ThumbnailFileStream { get; set; }

        #region Template

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonProperty("templates")]
        public List<MixTemplates.UpdateViewModel> Templates { get; set; }// Article Templates

        [JsonIgnore]
        public int ActivedTheme
        {
            get
            {
                return MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, Specificulture);
            }
        }

        [JsonIgnore]
        public string TemplateFolderType
        {
            get
            {
                return MixEnums.EnumTemplateFolder.Articles.ToString();
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, Specificulture)
                    , TemplateFolderType
                }
            );
            }
        }

        #endregion Template

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
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
        public string ThumbnailUrl
        {
            get
            {
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

        [JsonProperty("properties")]
        public List<ExtraProperty> Properties { get; set; }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }

        [JsonProperty("urlAliases")]
        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        [JsonProperty("columns")]
        public List<ModuleFieldViewModel> Columns { get; set; }
        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixArticle model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                ExtraFields = MixService.GetConfig<string>("DefaultArticleAttr");
            }
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            UrlAliases = GetAliases(_context, _transaction);
            if (!string.IsNullOrEmpty(this.Tags))
            {
                ListTag = JArray.Parse(this.Tags);
            }

            // Load Properties
            LoadExtraProperties();

            // Load Attribute Sets
            LoadAttributeSets(_context, _transaction);

            //Get Templates
            LoadTemplates(_context, _transaction);

            // Load Parent Pages
            LoadParentPage(_context,_transaction);

            // Load Parent Modules
            LoadParentModules(_context, _transaction);

            // Medias
            LoadMedias(_context, _transaction);

            // Sub Modules
            LoadSubModules(_context, _transaction);

            // Related Articles
            LoadRelatedArticle(_context, _transaction);
        }
        
        public override MixArticle ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            LastModified = DateTime.UtcNow;
            PublishedDateTime = PublishedDateTime?.ToUniversalTime();

            //  Parsing Extra Fields to json string
            var arrField = Columns != null ? JArray.Parse(
                Newtonsoft.Json.JsonConvert.SerializeObject(Columns.OrderBy(c => c.Priority).Where(
                    c => !string.IsNullOrEmpty(c.Name)))) : new JArray();
            ExtraFields = arrField.ToString(Newtonsoft.Json.Formatting.None);

            // Parsing Extra Properties value
            if (Properties != null && Properties.Count > 0)
            {
                JArray arrProperties = new JArray();
                foreach (var p in Properties.Where(p => !string.IsNullOrEmpty(p.Value) && !string.IsNullOrEmpty(p.Name)))
                {
                    arrProperties.Add(JObject.FromObject(p));
                }
                ExtraProperties = arrProperties.ToString(Formatting.None)?.Trim();
            }

            Template = View != null ? string.Format(@"{0}/{1}{2}", View.FolderType, View.FileName, View.Extension) : Template;
            if (ThumbnailFileStream != null)
            {
                string folder = CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.UploadFolder, "Articles", DateTime.UtcNow.ToString("dd-MM-yyyy")
                });
                string filename = CommonHelper.GetRandomName(ThumbnailFileStream.Name);
                bool saveThumbnail = CommonHelper.SaveFileBase64(folder, filename, ThumbnailFileStream.Base64);
                if (saveThumbnail)
                {
                    CommonHelper.RemoveFile(Thumbnail);
                    Thumbnail = CommonHelper.GetFullPath(new string[] { folder, filename });
                }
            }
            if (ImageFileStream != null)
            {
                string folder = CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.UploadFolder, "Articles", DateTime.UtcNow.ToString("dd-MM-yyyy")
                });
                string filename = CommonHelper.GetRandomName(ImageFileStream.Name);
                bool saveImage = CommonHelper.SaveFileBase64(folder, filename, ImageFileStream.Base64);
                if (saveImage)
                {
                    CommonHelper.RemoveFile(Image);
                    Image = CommonHelper.GetFullPath(new string[] { folder, filename });
                }
            }

            if (!string.IsNullOrEmpty(Image) && Image[0] == '/') { Image = Image.Substring(1); }
            if (!string.IsNullOrEmpty(Thumbnail) && Thumbnail[0] == '/') { Thumbnail = Thumbnail.Substring(1); }
            Tags = ListTag.ToString(Newtonsoft.Json.Formatting.None);
            GenerateSEO();

            return base.ParseModel(_context, _transaction);
        }

        #region Async Methods
        #region Save Sub Models Async
        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(
            MixArticle parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                // Save Template
                var saveTemplate = await View.SaveModelAsync(true, _context, _transaction);
                result.IsSucceed = result.IsSucceed && saveTemplate.IsSucceed;
                ViewModelHelper.HandleResult(saveTemplate, ref result);

                if (result.IsSucceed)
                {
                    // Save Alias
                    result = await SaveUrlAliasAsync(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Medias
                    result = await SaveMediasAsync(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Sub Modules
                    result = await SaveSubModulesAsync(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save related articles
                    result = await SaveRelatedArticleAsync(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Parent Category
                    result = await SaveParentPagesAsync(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save Parent Modules
                    result = await SaveParentModulesAsync(parent.Id, _context, _transaction);
                }

                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
        }
        private async Task<RepositoryResponse<bool>> SaveParentModulesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Modules)
            {
                item.ArticleId = id;
                item.Description = Title;
                item.Image = ThumbnailUrl;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await item.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveParentPagesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Pages)
            {
                item.ArticleId = id;
                item.Description = Title;
                item.Image = ThumbnailUrl;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await item.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveRelatedArticleAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navArticle in ArticleNavs)
            {
                navArticle.SourceId = id;
                navArticle.Status = MixEnums.MixContentStatus.Published;
                navArticle.Specificulture = Specificulture;
                if (navArticle.IsActived)
                {
                    var saveResult = await navArticle.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = await navArticle.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveSubModulesAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navModule in ModuleNavs)
            {
                navModule.ArticleId = id;
                navModule.Specificulture = Specificulture;
                navModule.Status = MixEnums.MixContentStatus.Published;
                if (navModule.IsActived)
                {
                    var saveResult = await navModule.SaveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    var saveResult = await navModule.RemoveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveMediasAsync(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navMedia in MediaNavs)
            {
                navMedia.ArticleId = id;
                navMedia.Specificulture = Specificulture;

                if (navMedia.IsActived)
                {
                    var saveResult = await navMedia.SaveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    var saveResult = await navMedia.RemoveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveUrlAliasAsync(int parentId, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in UrlAliases)
            {
                item.SourceId = parentId.ToString();
                item.Type = MixEnums.UrlAliasType.Article;
                item.Specificulture = Specificulture;
                var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
                if (!result.IsSucceed)
                {
                    break;
                }
            }
            return result;
        }

        #endregion
        
        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>()
            {
                IsSucceed = true
            };

            if (result.IsSucceed)
            {
                var navCate = await _context.MixPageArticle.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navCate)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navModule = await _context.MixModuleArticle.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navMedia = await _context.MixArticleMedia.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navMedia)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            if (result.IsSucceed)
            {
                var navModule = await _context.MixArticleModule.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navRelated = await _context.MixArticleMedia.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navRelated)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = await _context.MixUrlAlias.Where(n => n.SourceId == Id.ToString() && n.Type == (int)MixEnums.UrlAliasType.Article && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            result.IsSucceed = (await _context.SaveChangesAsync() > 0);
            return result;
        }

        public override Task<RepositoryResponse<List<UpdateViewModel>>> CloneAsync(MixArticle model, List<SupportedCulture> cloneCultures, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            return base.CloneAsync(model, cloneCultures, _context, _transaction);
        }
        #endregion Async Methods

        #region Sync Methods

        #region Save Sub Models
        public override RepositoryResponse<bool> SaveSubModels(
            MixArticle parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                // Save Template
                var saveTemplate = View.SaveModel(true, _context, _transaction);
                result.IsSucceed = result.IsSucceed && saveTemplate.IsSucceed;
                ViewModelHelper.HandleResult(saveTemplate, ref result);

                if (result.IsSucceed)
                {
                    // Save Alias
                    result = SaveUrlAlias(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Medias
                    result = SaveMedias(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Sub Modules
                    result = SaveSubModules(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save related articles
                    result = SaveRelatedArticle(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed)
                {
                    // Save Parent Category
                    result = SaveParentPages(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save Parent Modules
                    result = SaveParentModules(parent.Id, _context, _transaction);
                }

                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
        }
        private RepositoryResponse<bool> SaveParentModules(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Modules)
            {
                item.ArticleId = id;
                item.Description = Title;
                item.Image = ThumbnailUrl;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = item.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = item.RemoveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveParentPages(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Pages)
            {
                item.ArticleId = id;
                item.Description = Title;
                item.Image = ThumbnailUrl;
                item.Status = MixEnums.MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = item.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = item.RemoveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveRelatedArticle(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navArticle in ArticleNavs)
            {
                navArticle.SourceId = id;
                navArticle.Status = MixEnums.MixContentStatus.Published;
                navArticle.Specificulture = Specificulture;
                if (navArticle.IsActived)
                {
                    var saveResult = navArticle.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
                else
                {
                    var saveResult = navArticle.RemoveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveSubModules(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navModule in ModuleNavs)
            {
                navModule.ArticleId = id;
                navModule.Specificulture = Specificulture;
                navModule.Status = MixEnums.MixContentStatus.Published;
                if (navModule.IsActived)
                {
                    var saveResult = navModule.SaveModel(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    var saveResult = navModule.RemoveModel(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveMedias(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navMedia in MediaNavs)
            {
                navMedia.ArticleId = id;
                navMedia.Specificulture = Specificulture;

                if (navMedia.IsActived)
                {
                    var saveResult = navMedia.SaveModel(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    var saveResult = navMedia.RemoveModel(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveUrlAlias(int parentId, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in UrlAliases)
            {
                item.SourceId = parentId.ToString();
                item.Type = MixEnums.UrlAliasType.Article;
                item.Specificulture = Specificulture;
                var saveResult = item.SaveModel(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
                if (!result.IsSucceed)
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        public override RepositoryResponse<bool> RemoveRelatedModels(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>()
            {
                IsSucceed = true
            };

            if (result.IsSucceed)
            {
                var navCate = _context.MixPageArticle.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navCate)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navModule = _context.MixModuleArticle.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navMedia = _context.MixArticleMedia.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navMedia)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            if (result.IsSucceed)
            {
                var navModule = _context.MixArticleModule.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navRelated = _context.MixArticleMedia.Where(n => n.ArticleId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navRelated)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = _context.MixUrlAlias.Where(n => n.SourceId == Id.ToString() && n.Type == (int)MixEnums.UrlAliasType.Article && n.Specificulture == Specificulture).ToList();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            result.IsSucceed = (_context.SaveChanges() > 0);
            return result;
        }

        #endregion  Methods

        #endregion Overrides

        #region Expands

        private void LoadRelatedArticle(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            ArticleNavs = GetRelated(_context, _transaction);
            var otherArticles = MixArticles.ReadListItemViewModel.Repository.GetModelListBy(
                m => m.Id != Id && m.Specificulture == Specificulture
                    && !ArticleNavs.Any(n => n.SourceId == Id)
                    , "CreatedDateTime", 1, 10, 0, _context, _transaction);
            foreach (var item in otherArticles.Data.Items)
            {
                ArticleNavs.Add(new MixArticleArticles.ReadViewModel()
                {
                    SourceId = Id,
                    Image = item.ImageUrl,
                    DestinationId = item.Id,
                    Description = item.Title
                });
            }
        }

        private void LoadSubModules(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getArticleModule = MixArticleModules.ReadViewModel.Repository.GetModelListBy(
                n => n.ArticleId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getArticleModule.IsSucceed)
            {
                ModuleNavs = getArticleModule.Data.OrderBy(p => p.Priority).ToList();
                foreach (var item in ModuleNavs)
                {
                    item.IsActived = true;
                    item.Module.LoadData(articleId: Id, _context: _context, _transaction: _transaction);
                }
            }
            var otherModuleNavs = MixModules.ReadMvcViewModel.Repository.GetModelListBy(
                m => (m.Type == (int)MixEnums.MixModuleType.SubArticle) && m.Specificulture == Specificulture
                && !ModuleNavs.Any(n => n.ModuleId == m.Id), "CreatedDateTime", 1, null, 0, _context, _transaction);
            foreach (var item in otherModuleNavs.Data.Items)
            {
                item.LoadData(articleId: Id, _context: _context, _transaction: _transaction);
                ModuleNavs.Add(new MixArticleModules.ReadViewModel()
                {
                    ModuleId = item.Id,
                    Image = item.Image,
                    ArticleId = Id,
                    Description = item.Title,
                    Module = item
                });
            }
        }

        private void LoadAttributeSets(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getArticleAttributeSet = MixArticleAttributeSets.UpdateViewModel.Repository.GetModelListBy(
                n => n.ArticleId == Id, _context, _transaction);
            if (getArticleAttributeSet.IsSucceed)
            {
                AttributeSetNavs = getArticleAttributeSet.Data.OrderBy(p => p.Priority).ToList();
                foreach (var item in AttributeSetNavs)
                {
                    item.IsActived = true;
                    item.AttributeSet.ArticleData = MixAttributeSets.Helper.LoadArticleData(
                        articleId: Id, specificulture: Specificulture
                        , _context: _context, _transaction: _transaction).Data;
                }
            }
            var otherAttributeSetNavs = MixAttributeSets.UpdateViewModel.Repository.GetModelListBy(
                m => !AttributeSetNavs.Any(n => n.AttributeSetId == m.Id)
                    //&& (m.Type == (int)MixEnums.MixAttributeSetType.SubArticle)
                , "CreatedDateTime", 1, null, 0, _context, _transaction);
            foreach (var item in otherAttributeSetNavs.Data.Items)
            {
                //item.LoadData(articleId: Id, _context: _context, _transaction: _transaction);
                item.ArticleData = new PaginationModel<MixArticleAttributeValues.UpdateViewModel>();
                AttributeSetNavs.Add(new MixArticleAttributeSets.UpdateViewModel()
                {
                    AttributeSetId = item.Id,
                    ArticleId = Id,
                    Description = item.Title,
                    AttributeSet = item
                });
            }
        }

        private void LoadMedias(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getArticleMedia = MixArticleMedias.ReadViewModel.Repository.GetModelListBy(n => n.ArticleId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getArticleMedia.IsSucceed)
            {
                MediaNavs = getArticleMedia.Data.OrderBy(p => p.Priority).ToList();
                MediaNavs.ForEach(n => n.IsActived = true);
            }
        }

        private void LoadParentModules(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getModuleArticle = MixModuleArticles.ReadViewModel.GetModuleArticleNavAsync(Id, Specificulture, _context, _transaction);
            if (getModuleArticle.IsSucceed)
            {
                this.Modules = getModuleArticle.Data;
                this.Modules.ForEach(c =>
                {
                    c.IsActived = MixModuleArticles.ReadViewModel.Repository.CheckIsExists(n => n.ModuleId == c.ModuleId && n.ArticleId == Id, _context, _transaction);
                });
            }
            var otherModules = MixModules.ReadListItemViewModel.Repository.GetModelListBy(
                m => (m.Type == (int)MixEnums.MixModuleType.Content || m.Type == (int)MixEnums.MixModuleType.ListArticle)
                && m.Specificulture == Specificulture
                && !Modules.Any(n => n.ModuleId == m.Id && n.Specificulture == m.Specificulture)
                , "CreatedDateTime", 1, null, 0, _context, _transaction);
            foreach (var item in otherModules.Data.Items)
            {
                Modules.Add(new MixModuleArticles.ReadViewModel()
                {
                    ModuleId = item.Id,
                    Image = item.Image,
                    ArticleId = Id,
                    Description = Title
                });
            }

        }

        private void LoadParentPage(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getPageArticle = MixPageArticles.ReadViewModel.GetPageArticleNavAsync(Id, Specificulture, _context, _transaction);
            if (getPageArticle.IsSucceed)
            {
                this.Pages = getPageArticle.Data;
                this.Pages.ForEach(c =>
                {
                    c.IsActived = MixPageArticles.ReadViewModel.Repository.CheckIsExists(n => n.CategoryId == c.CategoryId && n.ArticleId == Id, _context, _transaction);
                });
            }
        }

        private void LoadTemplates(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            this.Templates = this.Templates ?? MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.TemplateFolderType).Data;
            View = MixTemplates.UpdateViewModel.GetTemplateByPath(Template, Specificulture, MixEnums.EnumTemplateFolder.Articles, _context, _transaction);

            this.Template = CommonHelper.GetFullPath(new string[]
               {
                    this.View?.FileFolder
                    , this.View?.FileName
               });

        }

        private void LoadExtraProperties()
        {
            // Parsing Extra Properties fields
            Columns = new List<ModuleFieldViewModel>();
            JArray arrField = !string.IsNullOrEmpty(ExtraFields) ? JArray.Parse(ExtraFields) : new JArray();
            foreach (var field in arrField)
            {
                ModuleFieldViewModel thisField = new ModuleFieldViewModel()
                {
                    Name = CommonHelper.ParseJsonPropertyName(field["name"].ToString()),
                    Title = field["title"]?.ToString(),
                    DefaultValue = field["defaultValue"]?.ToString(),
                    Options = field["options"] != null ? field["options"].Value<JArray>() : new JArray(),
                    Priority = field["priority"] != null ? field["priority"].Value<int>() : 0,
                    DataType = (MixDataType)(int)field["dataType"],
                    Width = field["width"] != null ? field["width"].Value<int>() : 3,
                    IsUnique = field["isUnique"] != null ? field["isUnique"].Value<bool>() : true,
                    IsRequired = field["isRequired"] != null ? field["isRequired"].Value<bool>() : true,
                    IsDisplay = field["isDisplay"] != null ? field["isDisplay"].Value<bool>() : true,
                    IsSelect = field["isSelect"] != null ? field["isSelect"].Value<bool>() : false,
                    IsGroupBy = field["isGroupBy"] != null ? field["isGroupBy"].Value<bool>() : false,
                };
                Columns.Add(thisField);
            }

            // Parsing Extra Properties value
            Properties = new List<ExtraProperty>();

            if (!string.IsNullOrEmpty(ExtraProperties))
            {
                JArray arr = JArray.Parse(ExtraProperties);
                foreach (JToken item in arr)
                {
                    var property = item.ToObject<ExtraProperty>();
                    Properties.Add(property);
                }
            }
        }

        List<SupportedCulture> LoadCultures(string initCulture = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getCultures = SystemCultureViewModel.Repository.GetModelList(_context, _transaction);
            var result = new List<SupportedCulture>();
            if (getCultures.IsSucceed)
            {
                foreach (var culture in getCultures.Data)
                {
                    result.Add(
                        new SupportedCulture()
                        {
                            Icon = culture.Icon,
                            Specificulture = culture.Specificulture,
                            Alias = culture.Alias,
                            FullName = culture.FullName,
                            Description = culture.FullName,
                            Id = culture.Id,
                            Lcid = culture.Lcid,
                            IsSupported = culture.Specificulture == initCulture || _context.MixArticle.Any(p => p.Id == Id && p.Specificulture == culture.Specificulture)
                        });

                }
            }
            return result;
        }

        private void GenerateSEO()
        {
            if (string.IsNullOrEmpty(this.SeoName))
            {
                this.SeoName = SeoHelper.GetSEOString(this.Title);
            }
            int i = 1;
            string name = SeoName;
            while (UpdateViewModel.Repository.CheckIsExists(a => a.SeoName == name && a.Specificulture == Specificulture && a.Id != Id))
            {
                name = SeoName + "_" + i;
                i++;
            }
            SeoName = name;

            if (string.IsNullOrEmpty(this.SeoTitle))
            {
                this.SeoTitle = SeoHelper.GetSEOString(this.Title);
            }

            if (string.IsNullOrEmpty(this.SeoDescription))
            {
                this.SeoDescription = SeoHelper.GetSEOString(this.Title);
            }

            if (string.IsNullOrEmpty(this.SeoKeywords))
            {
                this.SeoKeywords = SeoHelper.GetSEOString(this.Title);
            }
        }

        public List<MixArticleArticles.ReadViewModel> GetRelated(MixCmsContext context, IDbContextTransaction transaction)
        {
            var navs = MixArticleArticles.ReadViewModel.Repository.GetModelListBy(n => n.SourceId == Id && n.Specificulture == Specificulture, context, transaction).Data;
            navs.ForEach(n => n.IsActived = true);
            return navs.OrderBy(p => p.Priority).ToList();
        }
        public List<MixUrlAliases.UpdateViewModel> GetAliases(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixUrlAliases.UpdateViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture
                        && p.SourceId == Id.ToString() && p.Type == (int)MixEnums.UrlAliasType.Article, context, transaction);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return new List<MixUrlAliases.UpdateViewModel>();
            }
        }
        #endregion Expands
    }
}
