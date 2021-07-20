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
    public class SyncViewModel
         : ViewModelBase<MixCmsContext, MixPost, SyncViewModel>
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

        #region Template

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonProperty("templates")]
        public List<MixTemplates.UpdateViewModel> Templates { get; set; }// Post Templates

        [JsonIgnore]
        public int ActivedTheme {
            get {
                return MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, Specificulture);
            }
        }

        [JsonIgnore]
        public string TemplateFolderType {
            get {
                return MixTemplateFolders.Posts;
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder {
            get {
                return $"{MixFolders.TemplatesFolder}/" +
                  $"{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeName, Specificulture)}/" +
                  $"{MixTemplateFolders.Posts}";
            }
        }

        #endregion Template

        [JsonProperty("imageUrl")]
        public string ImageUrl {
            get {
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
        public string ThumbnailUrl {
            get {
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

        public SyncViewModel() : base()
        {
        }

        public SyncViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                ExtraFields = MixService.GetAppSetting<string>("DefaultPostAttr");
            }
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            UrlAliases = GetAliases(_context, _transaction);
            if (!string.IsNullOrEmpty(this.Tags))
            {
                ListTag = JArray.Parse(this.Tags);
            }

            // Parsing Extra Properties fields
            Columns = new List<ModuleFieldViewModel>();
            JArray arrField = !string.IsNullOrEmpty(ExtraFields) ? JArray.Parse(ExtraFields) : new JArray();
            foreach (var field in arrField)
            {
                ModuleFieldViewModel thisField = new ModuleFieldViewModel()
                {
                    Name = MixCommonHelper.ParseJsonPropertyName(field["name"].ToString()),
                    Title = field["title"]?.ToString(),
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
                    Properties.Add(item.ToObject<ExtraProperty>());
                }
            }
            //Get Templates
            this.Templates = this.Templates ?? MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.TemplateFolderType).Data;
            View = MixTemplates.UpdateViewModel.GetTemplateByPath(Template, Specificulture, MixTemplateFolders.Posts, _context, _transaction);

            this.Template = $"{this.View?.FileFolder}/{this.View?.FileName}";

            var getPagePost = MixPagePosts.ReadViewModel.GetPagePostNavAsync(Id, Specificulture, _context, _transaction);
            if (getPagePost.IsSucceed)
            {
                this.Pages = getPagePost.Data;
                this.Pages.ForEach(c =>
                {
                    c.IsActived = MixPagePosts.ReadViewModel.Repository.CheckIsExists(n => n.PageId == c.PageId && n.PostId == Id, _context, _transaction);
                });
            }

            var getModulePost = MixModulePosts.ReadViewModel.GetModulePostNavAsync(Id, Specificulture, _context, _transaction);
            if (getModulePost.IsSucceed)
            {
                this.Modules = getModulePost.Data;
                this.Modules.ForEach(c =>
                {
                    c.IsActived = MixModulePosts.ReadViewModel.Repository.CheckIsExists(n => n.ModuleId == c.ModuleId && n.PostId == Id, _context, _transaction);
                });
            }
            var otherModules = MixModules.ReadListItemViewModel.Repository.GetModelListBy(
                m => (m.Type == (int)MixModuleType.Content || m.Type == (int)MixModuleType.ListPost)
                && m.Specificulture == Specificulture
                && !Modules.Any(n => n.ModuleId == m.Id && n.Specificulture == m.Specificulture)
                , "CreatedDateTime", Heart.Enums.DisplayDirection.Desc, null, 0, _context, _transaction);
            foreach (var item in otherModules.Data.Items)
            {
                Modules.Add(new MixModulePosts.ReadViewModel()
                {
                    ModuleId = item.Id,
                    Image = item.Image,
                    PostId = Id,
                    Description = Title
                });
            }

            // Medias
            var getPostMedia = MixPostMedias.ReadViewModel.Repository.GetModelListBy(n => n.PostId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getPostMedia.IsSucceed)
            {
                MediaNavs = getPostMedia.Data.OrderBy(p => p.Priority).ToList();
                MediaNavs.ForEach(n => n.IsActived = true);
            }
            // Modules
            var getPostModule = MixPostModules.ReadViewModel.Repository.GetModelListBy(
                n => n.PostId == Id && n.Specificulture == Specificulture, _context, _transaction);
            if (getPostModule.IsSucceed)
            {
                ModuleNavs = getPostModule.Data.OrderBy(p => p.Priority).ToList();
                foreach (var item in ModuleNavs)
                {
                    item.IsActived = true;
                    item.Module.LoadData(postId: Id, _context: _context, _transaction: _transaction);
                }
            }

            // Related Posts
            PostNavs = GetRelated(_context, _transaction);
            var otherPosts = MixPosts.ReadListItemViewModel.Repository.GetModelListBy(
                m => m.Id != Id && m.Specificulture == Specificulture
                    && !PostNavs.Any(n => n.SourceId == Id)
                    , "CreatedDateTime", Heart.Enums.DisplayDirection.Desc, 10, 0, _context, _transaction);
            foreach (var item in otherPosts.Data.Items)
            {
                PostNavs.Add(new MixPostPosts.ReadViewModel()
                {
                    SourceId = Id,
                    Image = item.ImageUrl,
                    DestinationId = item.Id,
                    Description = item.Title
                });
            }
        }

        public override MixPost ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
            GenerateSEO();

            return base.ParseModel(_context, _transaction);
        }

        #region Async Methods

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(
            MixPost parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                if (result.IsSucceed)
                {
                    foreach (var item in UrlAliases)
                    {
                        item.SourceId = parent.Id.ToString();
                        item.Type = MixUrlAliasType.Post;
                        item.Specificulture = Specificulture;
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Exception = saveResult.Exception;
                            result.Errors.AddRange(saveResult.Errors);
                            break;
                        }
                    }
                }
                if (result.IsSucceed)
                {
                    var startMediaId = MixMedias.UpdateViewModel.Repository.Max(c => c.Id, _context, _transaction).Data;
                    foreach (var navMedia in MediaNavs)
                    {
                        if (navMedia.Media != null)
                        {
                            startMediaId += 1;
                            navMedia.Media.Specificulture = Specificulture;
                            navMedia.Media.Id = startMediaId;
                            var saveMedia = await navMedia.Media.SaveModelAsync(false, _context, _transaction);
                            if (saveMedia.IsSucceed)
                            {
                                navMedia.PostId = parent.Id;
                                navMedia.MediaId = saveMedia.Data.Model.Id;
                                navMedia.Specificulture = parent.Specificulture;
                                var saveResult = await navMedia.SaveModelAsync(false, _context, _transaction);
                                result.IsSucceed = saveResult.IsSucceed;
                                if (!result.IsSucceed)
                                {
                                    result.Exception = saveResult.Exception;
                                    Errors.AddRange(saveResult.Errors);
                                }
                            }
                            else
                            {
                                result.IsSucceed = false;
                                result.Exception = saveMedia.Exception;
                                Errors.AddRange(saveMedia.Errors);
                            }
                        }
                    }
                }
                if (result.IsSucceed)
                {
                    foreach (var navModule in ModuleNavs)
                    {
                        navModule.PostId = parent.Id;
                        navModule.Specificulture = parent.Specificulture;
                        navModule.Status = MixContentStatus.Published;
                        if (navModule.IsActived)
                        {
                            var saveResult = await navModule.SaveModelAsync(false, _context, _transaction);
                            result.IsSucceed = saveResult.IsSucceed;
                            if (!result.IsSucceed)
                            {
                                result.Exception = saveResult.Exception;
                                Errors.AddRange(saveResult.Errors);
                            }
                        }
                        else
                        {
                            var saveResult = await navModule.RemoveModelAsync(false, _context, _transaction);
                            result.IsSucceed = saveResult.IsSucceed;
                            if (!result.IsSucceed)
                            {
                                result.Exception = saveResult.Exception;
                                Errors.AddRange(saveResult.Errors);
                            }
                        }
                    }
                }

                if (result.IsSucceed)
                {
                    foreach (var navPost in PostNavs)
                    {
                        navPost.SourceId = parent.Id;
                        navPost.Status = MixContentStatus.Published;
                        navPost.Specificulture = parent.Specificulture;
                        if (navPost.IsActived)
                        {
                            var saveResult = await navPost.SaveModelAsync(false, _context, _transaction);
                            result.IsSucceed = saveResult.IsSucceed;
                            if (!result.IsSucceed)
                            {
                                result.Exception = saveResult.Exception;
                                Errors.AddRange(saveResult.Errors);
                            }
                        }
                        else
                        {
                            var saveResult = await navPost.RemoveModelAsync(false, _context, _transaction);
                            result.IsSucceed = saveResult.IsSucceed;
                            if (!result.IsSucceed)
                            {
                                result.Exception = saveResult.Exception;
                                Errors.AddRange(saveResult.Errors);
                            }
                        }
                    }
                }
                if (result.IsSucceed)
                {
                    // Save Parent Category
                    foreach (var item in Pages)
                    {
                        item.PostId = parent.Id;
                        item.Description = parent.Title;
                        item.Image = ThumbnailUrl;
                        item.Status = MixContentStatus.Published;
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
                }

                if (result.IsSucceed)
                {
                    // Save Parent Modules
                    foreach (var item in Modules)
                    {
                        item.PostId = parent.Id;
                        item.Description = parent.Title;
                        item.Image = ThumbnailUrl;
                        item.Status = MixContentStatus.Published;
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

        #endregion Async Methods

        #region Sync Methods

        public override RepositoryResponse<bool> SaveSubModels(
            MixPost parent
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                if (result.IsSucceed)
                {
                    var startMediaId = MixMedias.UpdateViewModel.Repository.Max(c => c.Id, _context, _transaction).Data;
                    foreach (var navMedia in MediaNavs)
                    {
                        if (navMedia.Media != null)
                        {
                            startMediaId += 1;
                            navMedia.Media.Specificulture = Specificulture;
                            navMedia.Media.Id = startMediaId;
                            var saveMedia = navMedia.Media.SaveModel(false, _context, _transaction);
                            if (saveMedia.IsSucceed)
                            {
                                navMedia.PostId = parent.Id;
                                navMedia.MediaId = saveMedia.Data.Model.Id;
                                navMedia.Specificulture = parent.Specificulture;
                                var saveResult = navMedia.SaveModel(false, _context, _transaction);
                                result.IsSucceed = saveResult.IsSucceed;
                                if (!result.IsSucceed)
                                {
                                    result.Exception = saveResult.Exception;
                                    Errors.AddRange(saveResult.Errors);
                                }
                            }
                            else
                            {
                                result.IsSucceed = false;
                                result.Exception = saveMedia.Exception;
                                Errors.AddRange(saveMedia.Errors);
                            }
                        }
                    }
                }
                if (result.IsSucceed)
                {
                    foreach (var navModule in ModuleNavs)
                    {
                        navModule.PostId = parent.Id;
                        navModule.Specificulture = parent.Specificulture;
                        navModule.Status = MixContentStatus.Published;
                        if (navModule.IsActived)
                        {
                            var saveResult = navModule.SaveModel(true, _context, _transaction);
                            result.IsSucceed = saveResult.IsSucceed;
                            if (!result.IsSucceed)
                            {
                                result.Exception = saveResult.Exception;
                                Errors.AddRange(saveResult.Errors);
                            }
                        }
                        else
                        {
                            var saveResult = navModule.RemoveModel(false, _context, _transaction);
                            result.IsSucceed = saveResult.IsSucceed;
                            if (!result.IsSucceed)
                            {
                                result.Exception = saveResult.Exception;
                                Errors.AddRange(saveResult.Errors);
                            }
                        }
                    }
                }

                if (result.IsSucceed)
                {
                    // Save Parent Category
                    foreach (var item in Pages)
                    {
                        item.PostId = parent.Id;
                        item.Description = parent.Title;
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
                }

                if (result.IsSucceed)
                {
                    // Save Parent Modules
                    foreach (var item in Modules)
                    {
                        item.PostId = parent.Id;
                        item.Description = parent.Title;
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

        public override RepositoryResponse<bool> RemoveRelatedModels(SyncViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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

        public List<MixPostPosts.ReadViewModel> GetRelated(MixCmsContext context, IDbContextTransaction transaction)
        {
            var navs = MixPostPosts.ReadViewModel.Repository.GetModelListBy(n => n.SourceId == Id && n.Specificulture == Specificulture, context, transaction).Data;
            navs.ForEach(n => n.IsActived = true);
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