using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixCultures;
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
    public class UpdateViewModel
         : ViewModelBase<MixCmsContext, MixPost, UpdateViewModel>
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

        [JsonProperty("editorValue")]
        public string EditorValue { get; set; }

        [JsonProperty("editorType")]
        public MixEditorType? EditorType { get; set; }

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

        [JsonProperty("isClone")]
        public bool IsClone { get; set; }

        [JsonProperty("templatePath")]
        public string TemplatePath { get; set; }

        [JsonProperty("domain")]
        public string Domain => MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain);

        [JsonProperty("categories")]
        public List<MixPagePosts.ReadViewModel> Pages { get; set; }

        [JsonProperty("modules")]
        public List<MixModulePosts.ReadViewModel> Modules { get; set; } // Parent to Modules

        [JsonProperty("mediaNavs")]
        public List<MixPostMedias.ReadViewModel> MediaNavs { get; set; }

        [JsonProperty("moduleNavs")]
        public List<MixPostModules.ReadViewModel> ModuleNavs { get; set; }

        [JsonProperty("postNavs")]
        public List<MixPostPosts.ReadViewModel> PostNavs { get; set; }

        [JsonProperty("listTag")]
        public JArray ListTag { get; set; } = new JArray();

        [JsonProperty("imageFileStream")]
        public FileStreamViewModel ImageFileStream { get; set; }

        [JsonProperty("thumbnailFileStream")]
        public FileStreamViewModel ThumbnailFileStream { get; set; }

        [JsonProperty("sysCategories")]
        public List<MixDatabaseDataAssociations.FormViewModel> SysCategories { get; set; }

        [JsonProperty("sysTags")]
        public List<MixDatabaseDataAssociations.FormViewModel> SysTags { get; set; }

        #region Template

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonProperty("templates")]
        public List<MixTemplates.UpdateViewModel> Templates { get; set; }// Post Templates

        [JsonIgnore]
        public int ActivedTheme
        {
            get
            {
                return MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, Specificulture);
            }
        }

        [JsonIgnore]
        public string TemplateFolderType
        {
            get
            {
                return MixTemplateFolders.Posts;
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/" +
                  $"{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeName, Specificulture)}/" +
                  $"{MixTemplateFolders.Posts}";
            }
        }

        #endregion Template

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1))
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
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1)
                {
                    return $"{Domain.TrimEnd('/')}/{Thumbnail.TrimStart('/')}";
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

        public UpdateViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Type = string.IsNullOrEmpty(Type) ? MixConstants.MixDatabaseName.ADDITIONAL_COLUMN_POST : Type;
            EditorValue ??= Content;
            EditorType ??= MixEditorType.Html;
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            UrlAliases = GetAliases(_context, _transaction);
            if (!string.IsNullOrEmpty(this.Tags))
            {
                ListTag = JArray.Parse(this.Tags);
            }

            LoadAttributes(_context, _transaction);

            //Get Templates
            LoadTemplates(_context, _transaction);

            // Load Parent Pages
            LoadParentPage(_context, _transaction);

            // Load Parent Modules
            LoadParentModules(_context, _transaction);

            // Medias
            LoadMedias(_context, _transaction);

            // Related Posts
            LoadRelatedPost(_context, _transaction);

            DetailsUrl = Id > 0
                   ? MixCmsHelper.GetDetailsUrl(Specificulture, $"/{MixService.GetConfig("PostController", Specificulture, "post")}/{Id}/{SeoName}")
                   : null;
        }

        public override MixPost ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            LastModified = DateTime.UtcNow;
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
                string folder = MixCmsHelper.GetUploadFolder(Specificulture);
                string filename = MixCommonHelper.GetRandomName(ThumbnailFileStream.Name);
                bool saveThumbnail = MixCommonHelper.SaveFileBase64(folder, filename, ThumbnailFileStream.Base64);
                if (saveThumbnail)
                {
                    MixCommonHelper.RemoveFile(Thumbnail);
                    Thumbnail = $"{folder}/{filename}";
                }
            }
            if (ImageFileStream != null)
            {
                string folder = MixCmsHelper.GetUploadFolder(Specificulture);
                string filename = MixCommonHelper.GetRandomName(ImageFileStream.Name);
                bool saveImage = MixCommonHelper.SaveFileBase64(folder, filename, ImageFileStream.Base64);
                if (saveImage)
                {
                    MixCommonHelper.RemoveFile(Image);
                    Image = $"{folder}/{filename}";
                }
            }

            if (!string.IsNullOrEmpty(Image) && Image[0] == '/') { Image = Image.Substring(1); }
            if (!string.IsNullOrEmpty(Thumbnail) && Thumbnail[0] == '/') { Thumbnail = Thumbnail.Substring(1); }
            Tags = ListTag.ToString(Newtonsoft.Json.Formatting.None);
            GenerateSEO(_context, _transaction);

            return base.ParseModel(_context, _transaction);
        }

        #region Async Methods

        #region Save Sub Models Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(
            MixPost parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                // Save Template
                if (View.Id == 0)
                {
                    var saveTemplate = await View.SaveModelAsync(true, _context, _transaction);
                    ViewModelHelper.HandleResult(saveTemplate, ref result);
                }

                if (result.IsSucceed)
                {
                    // Save Alias
                    result = await SaveUrlAliasAsync(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed && MediaNavs != null)
                {
                    // Save Medias
                    result = await SaveMediasAsync(parent.Id, _context, _transaction);
                }
                //if (result.IsSucceed)
                //{
                //    // Save Sub Modules
                //    result = await SaveSubModulesAsync(parent.Id, _context, _transaction);
                //}

                if (result.IsSucceed)
                {
                    // Save Attributes
                    result = await SaveAttributeAsync(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed && PostNavs != null)
                {
                    // Save related posts
                    result = await SaveRelatedPostAsync(parent.Id, _context, _transaction);
                }
                if (result.IsSucceed && Pages != null)
                {
                    // Save Parent Category
                    result = await SaveParentPagesAsync(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed && Modules != null)
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

        private async Task<RepositoryResponse<bool>> SaveAttributeAsync(int parentId, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            foreach (var item in SysCategories)
            {
                // Force create new sub models if parent is create new
                if (item.ParentId != parentId.ToString())
                {
                    item.Id = null;
                }
                if (result.IsSucceed)
                {
                    item.ParentId = parentId.ToString();
                    item.ParentType = MixDatabaseParentType.Post;
                    item.Specificulture = Specificulture;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }

            foreach (var item in SysTags)
            {
                // Force create new sub models if parent is create new
                if (item.ParentId != parentId.ToString())
                {
                    item.Id = null;
                }
                if (result.IsSucceed)
                {
                    item.ParentId = parentId.ToString();
                    item.ParentType = MixDatabaseParentType.Post;
                    item.Specificulture = Specificulture;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveParentModulesAsync(int newPostId, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Modules)
            {
                // Force create new sub models if parent is create new
                if (item.PostId != newPostId)
                {
                    item.Id = 0;
                }
                item.Specificulture = Specificulture;
                item.PostId = newPostId;
                item.Status = MixContentStatus.Published;
                if (item.IsActived)
                {
                    if (!_context.MixModulePost.Any(m => m.Specificulture == item.Specificulture && m.PostId == item.PostId && m.ModuleId == item.ModuleId))
                    {
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Exception = saveResult.Exception;
                            Errors.AddRange(saveResult.Errors);
                        }
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

        private async Task<RepositoryResponse<bool>> SaveParentPagesAsync(int newPostId, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Pages)
            {
                // Force create new sub models if parent is create new
                if (item.PostId != newPostId)
                {
                    item.Id = 0;
                }
                item.Specificulture = Specificulture;
                item.PostId = newPostId;
                item.Status = MixContentStatus.Published;
                if (item.IsActived)
                {
                    if (!_context.MixPagePost.Any(m => m.Specificulture == item.Specificulture && m.PostId == item.PostId && m.PageId == item.PageId))
                    {
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Exception = saveResult.Exception;
                            Errors.AddRange(saveResult.Errors);
                        }
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

        private async Task<RepositoryResponse<bool>> SaveRelatedPostAsync(int newPostId, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in PostNavs)
            {
                // Force create new sub models if parent is create new
                if (item.SourceId != newPostId)
                {
                    item.Id = 0;
                }
                item.SourceId = newPostId;
                item.Status = MixContentStatus.Published;
                item.Specificulture = Specificulture;
                if (item.IsActived)
                {
                    if (!_context.MixRelatedPost.Any(m => m.Specificulture == item.Specificulture && m.SourceId == item.SourceId && m.DestinationId == item.DestinationId))
                    {
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Exception = saveResult.Exception;
                            Errors.AddRange(saveResult.Errors);
                        }
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

        private async Task<RepositoryResponse<bool>> SaveMediasAsync(int newPostid, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navMedia in MediaNavs)
            {
                // Force create new sub models if parent is create new
                if (navMedia.PostId != newPostid)
                {
                    navMedia.Id = 0;
                }
                navMedia.PostId = newPostid;
                navMedia.Specificulture = Specificulture;

                if (navMedia.IsActived)
                {
                    if (navMedia.Id == 0)
                    {
                        var saveResult = await navMedia.SaveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
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
                // Force create new sub models if parent is create new
                if (item.SourceId != parentId.ToString())
                {
                    item.Id = 0;
                }
                item.SourceId = parentId.ToString();
                item.Type = MixUrlAliasType.Post;
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

        #endregion Save Sub Models Async

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string parentId = Id.ToString();
            RepositoryResponse<bool> result = new RepositoryResponse<bool>()
            {
                IsSucceed = true
            };

            if (result.IsSucceed)
            {
                var removeAdditionalData = await MixDatabaseDataAssociations.UpdateViewModel.Repository.RemoveListModelAsync(
                    true,
                    m => m.ParentId == parentId
                        && m.MixDatabaseName == MixDatabaseNames.ADDITIONAL_COLUMN_POST
                        && m.ParentType == MixDatabaseParentType.Post
                        && m.Specificulture == Specificulture,
                    _context, _transaction);
                ViewModelHelper.HandleResult(removeAdditionalData, ref result);
            }

            if (result.IsSucceed)
            {
                var navCate = await _context.MixPagePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navCate)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navModule = await _context.MixModulePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navMedia = await _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navMedia)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            if (result.IsSucceed)
            {
                var navModule = await _context.MixPostModule.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navRelated = await _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navRelated)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = await _context.MixUrlAlias.Where(n => n.SourceId == Id.ToString() && n.Type == (int)MixUrlAliasType.Post && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            await _context.SaveChangesAsync();
            return result;
        }

        public override async Task<RepositoryResponse<bool>> CloneSubModelsAsync(MixPost parent, List<SupportedCulture> cloneCultures, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string parentId = Id.ToString();
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var getAdditionalData = await MixDatabaseDataAssociations.UpdateViewModel.Repository.GetFirstModelAsync(
                    m => m.ParentId == parentId && m.ParentType == MixDatabaseParentType.Post && m.Specificulture == Specificulture,
                    _context, _transaction);
            if (getAdditionalData.IsSucceed)
            {
                getAdditionalData.Data.Cultures = Cultures;
                var model = getAdditionalData.Data.ParseModel();
                var cloneData = await getAdditionalData.Data.CloneAsync(model, Cultures, _context, _transaction);
                ViewModelHelper.HandleResult(cloneData, ref result);
            }
            return result;
        }

        #endregion Async Methods

        #region Sync Methods

        #region Save Sub Models

        public override RepositoryResponse<bool> SaveSubModels(
            MixPost parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                // Save Template
                if (View.Id == 0)
                {
                    var saveTemplate = View.SaveModel(true, _context, _transaction);
                    ViewModelHelper.HandleResult(saveTemplate, ref result);
                }

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
                //if (result.IsSucceed)
                //{
                //    // Save Sub Modules
                //    result = SaveSubModules(parent.Id, _context, _transaction);
                //}

                if (result.IsSucceed)
                {
                    // Save Attributes
                    result = SaveAttribute(parent.Id, _context, _transaction);
                }

                if (result.IsSucceed)
                {
                    // Save related posts
                    result = SaveRelatedPost(parent.Id, _context, _transaction);
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

        private RepositoryResponse<bool> SaveAttribute(int parentId, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in SysCategories)
            {
                if (result.IsSucceed)
                {
                    item.ParentId = parentId.ToString();
                    item.ParentType = MixDatabaseParentType.Post;
                    item.Specificulture = Specificulture;
                    var saveResult = item.SaveModel(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }

            foreach (var item in SysTags)
            {
                if (result.IsSucceed)
                {
                    item.ParentId = parentId.ToString();
                    item.ParentType = MixDatabaseParentType.Post;
                    item.Specificulture = Specificulture;
                    var saveResult = item.SaveModel(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }

            return result;
        }

        private RepositoryResponse<bool> SaveParentModules(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Modules)
            {
                item.PostId = id;
                item.Description = Title;
                item.Image = ThumbnailUrl;
                item.Status = MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = item.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                    else
                    {
                        MixModules.ReadListItemViewModel.Repository.RemoveCacheAsync(item.Module.Model, _context, _transaction);
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
                    else
                    {
                        MixModules.ReadListItemViewModel.Repository.RemoveCacheAsync(item.Module.Model, _context, _transaction);
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
                item.PostId = id;
                item.Description = Title;
                item.Image = ThumbnailUrl;
                item.Status = MixContentStatus.Published;
                if (item.IsActived)
                {
                    var saveResult = item.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                    else
                    {
                        MixPages.ReadListItemViewModel.Repository.RemoveCacheAsync(item.Page.Model, _context, _transaction);
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
                    else
                    {
                        MixPages.ReadListItemViewModel.Repository.RemoveCacheAsync(item.Page.Model, _context, _transaction);
                    }
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveRelatedPost(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navPost in PostNavs)
            {
                navPost.SourceId = id;
                navPost.Status = MixContentStatus.Published;
                navPost.Specificulture = Specificulture;
                if (navPost.IsActived)
                {
                    var saveResult = navPost.SaveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                    else
                    {
                        MixPosts.ReadViewModel.Repository.RemoveCacheAsync(navPost.RelatedPost.Model);
                    }
                }
                else
                {
                    var saveResult = navPost.RemoveModel(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        Errors.AddRange(saveResult.Errors);
                    }
                    else
                    {
                        MixPosts.ReadViewModel.Repository.RemoveCacheAsync(navPost.RelatedPost.Model);
                    }
                }
            }
            return result;
        }

        private RepositoryResponse<bool> SaveMedias(int id, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var navMedia in MediaNavs)
            {
                navMedia.PostId = id;
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
                item.Type = MixUrlAliasType.Post;
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

        #endregion Save Sub Models

        public override RepositoryResponse<bool> RemoveRelatedModels(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>()
            {
                IsSucceed = true
            };

            if (result.IsSucceed)
            {
                var navCate = _context.MixPagePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navCate)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navModule = _context.MixModulePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navMedia = _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navMedia)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            if (result.IsSucceed)
            {
                var navModule = _context.MixPostModule.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navRelated = _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToList();
                foreach (var item in navRelated)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = _context.MixUrlAlias.Where(n => n.SourceId == Id.ToString() && n.Type == (int)MixUrlAliasType.Post && n.Specificulture == Specificulture).ToList();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            result.IsSucceed = (_context.SaveChanges() > 0);
            return result;
        }

        #endregion Sync Methods

        #endregion Overrides

        #region Expands

        private void LoadRelatedPost(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            PostNavs = GetRelated(_context, _transaction);
        }

        private void LoadMedias(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getPostMedia = MixPostMedias.ReadViewModel.Repository.GetModelListBy(n => n.PostId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getPostMedia.IsSucceed)
            {
                MediaNavs = getPostMedia.Data.OrderBy(p => p.Priority).ToList();
                MediaNavs.ForEach(n => { n.Specificulture = Specificulture; n.IsActived = true; });
            }
        }

        private void LoadParentModules(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getModulePost = MixModulePosts.ReadViewModel.GetModulePostNavAsync(Id, Specificulture, _context, _transaction);
            if (getModulePost.IsSucceed)
            {
                Modules = getModulePost.Data;
                Modules.ForEach(c =>
                {
                    c.Specificulture = Specificulture;
                    c.Description = c.Module?.Title ?? c.Description;
                    c.IsActived = MixModulePosts.ReadViewModel.Repository.CheckIsExists(n => n.ModuleId == c.ModuleId && n.PostId == Id, _context, _transaction);
                });
            }
        }

        private void LoadParentPage(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getPagePost = MixPagePosts.ReadViewModel.GetPagePostNavAsync(Id, Specificulture, _context, _transaction);
            if (getPagePost.IsSucceed)
            {
                this.Pages = getPagePost.Data;
                this.Pages.ForEach(c =>
                {
                    c.Specificulture = Specificulture;
                    c.Description = c.Page?.Title ?? c.Description;
                    c.IsActived = MixPagePosts.ReadViewModel.Repository.CheckIsExists(n => n.PageId == c.PageId && n.PostId == Id, _context, _transaction);
                });
            }
        }

        private void LoadTemplates(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            this.Templates = this.Templates ?? MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.TemplateFolderType, _context, _transaction).Data;
            View = MixTemplates.UpdateViewModel.GetTemplateByPath(Template, Specificulture, MixTemplateFolders.Posts, _context, _transaction);
            this.Template = $"{this.View?.TemplateFolder}/{this.View?.FolderType}/{this.View?.FileName}{this.View?.Extension}";
        }

        private List<SupportedCulture> LoadCultures(string initCulture = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
                            IsSupported = culture.Specificulture == initCulture || _context.MixPost.Any(p => p.Id == Id && p.Specificulture == culture.Specificulture)
                        });
                }
            }
            return result;
        }

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var getCategories = MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture
                && m.ParentId == Id.ToString() && m.ParentType == MixDatabaseParentType.Post
                && m.MixDatabaseName == MixConstants.MixDatabaseName.SYSTEM_CATEGORY, _context, _transaction);
            if (getCategories.IsSucceed)
            {
                getCategories.Data.ForEach(t => t.IsActived = true);
                SysCategories = getCategories.Data;
            }

            var getTags = MixDatabaseDataAssociations.FormViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture
                && m.ParentId == Id.ToString() && m.ParentType == MixDatabaseParentType.Post
                && m.MixDatabaseName == MixConstants.MixDatabaseName.SYSTEM_TAG, _context, _transaction);
            if (getTags.IsSucceed)
            {
                getTags.Data.ForEach(t => t.IsActived = true);
                SysTags = getTags.Data;
            }
        }

        private void GenerateSEO(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(this.SeoName))
            {
                this.SeoName = SeoHelper.GetSEOString(this.Title);
            }
            int i = 1;
            string name = SeoName;
            while (UpdateViewModel.Repository.CheckIsExists(a => a.SeoName == name && a.Specificulture == Specificulture && a.Id != Id, _context, _transaction))
            {
                name = SeoName + "_" + i;
                i++;
            }
            SeoName = name;

            if (string.IsNullOrEmpty(this.SeoTitle))
            {
                this.SeoTitle = this.Title;
            }

            if (string.IsNullOrEmpty(this.Excerpt))
            {
                this.SeoDescription = this.Excerpt;
            }

            if (string.IsNullOrEmpty(this.SeoKeywords))
            {
                this.SeoKeywords = SeoHelper.GetSEOString(this.Title);
            }
        }

        public List<MixPostPosts.ReadViewModel> GetRelated(MixCmsContext context, IDbContextTransaction transaction)
        {
            var navs = MixPostPosts.ReadViewModel.Repository.GetModelListBy(n => n.SourceId == Id && n.Specificulture == Specificulture, context, transaction).Data;
            navs.ForEach(n => { n.Specificulture = Specificulture; n.IsActived = true; });
            return navs.OrderBy(p => p.Priority).ToList();
        }

        public List<MixUrlAliases.UpdateViewModel> GetAliases(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixUrlAliases.UpdateViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture
                        && p.SourceId == Id.ToString() && p.Type == (int)MixUrlAliasType.Post, context, transaction);
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