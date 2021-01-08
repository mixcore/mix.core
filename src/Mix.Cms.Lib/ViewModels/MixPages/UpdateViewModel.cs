﻿using Microsoft.EntityFrameworkCore.Storage;
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
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixPages
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixPage, UpdateViewModel>
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

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("cssClass")]
        public string CssClass { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

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
        public MixPageType Type { get; set; }
        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("staticUrl")]
        public string StaticUrl { get; set; }

        [JsonProperty("level")]
        public int? Level { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        [JsonProperty("createdBy")]
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

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get => Id > 0 ? $"/page/{Specificulture}/{SeoName}" : null; }

        [JsonProperty("moduleNavs")]
        public List<MixPageModules.ReadMvcViewModel> ModuleNavs { get; set; } // Parent to Modules

        [JsonProperty("listTag")]
        public JArray ListTag { get; set; } = new JArray();

        [JsonProperty("imageFileStream")]
        public FileStreamViewModel ImageFileStream { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

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

        #region Template

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonProperty("templates")]
        public List<MixTemplates.UpdateViewModel> Templates { get; set; }

        [JsonProperty("master")]
        public MixTemplates.UpdateViewModel Master { get; set; }

        [JsonProperty("masters")]
        public List<MixTemplates.UpdateViewModel> Masters { get; set; }

        [JsonIgnore]
        public int ActivedTheme
        {
            get
            {
                return MixService.GetConfig<int>(AppSettingKeywords.ThemeId, Specificulture);
            }
        }

        [JsonIgnore]
        public MixTemplateFolderType TemplateFolderType
        {
            get
            {
                return MixTemplateFolderType.Pages;
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/{MixService.GetConfig<string>(AppSettingKeywords.ThemeName, Specificulture)}/{TemplateFolderType}";
            }
        }

        #endregion Template

        [JsonProperty("urlAliases")]
        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        [JsonProperty("sysCategories")]
        public List<MixRelatedAttributeDatas.UpdateViewModel> SysCategories { get; set; }

        [JsonProperty("sysTags")]
        public List<MixRelatedAttributeDatas.UpdateViewModel> SysTags { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixPage ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            GenerateSEO();

            //var navParent = ParentNavs?.FirstOrDefault(p => p.IsActived);

            //if (navParent != null)
            //{
            //    Level = 1; //Repository.GetSingleModel(c => c.Id == navParent.ParentId, _context, _transaction).Data.Level + 1;
            //}
            //else
            //{
            //    Level = 0;
            //}

            Template = View != null ? $"{View.FolderType}/{View.FileName}{View.Extension}" : Template;
            Layout = Master != null ? $"{Master.FolderType}/{Master.FileName}{Master.Extension}" : null;
            if (Id == 0)
            {
                Id = Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            LastModified = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(Image) && Image[0] == '/') { Image = Image.Substring(1); }
            if (!string.IsNullOrEmpty(Thumbnail) && Thumbnail[0] == '/') { Thumbnail = Thumbnail.Substring(1); }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Cultures = Helper.LoadCultures(Id, Specificulture, _context, _transaction);
            if (!string.IsNullOrEmpty(this.Tags))
            {
                ListTag = JArray.Parse(this.Tags);
            }

            // Load page views
            this.Templates = MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.TemplateFolderType, _context, _transaction).Data;
            var templateName = Template?.Substring(Template.LastIndexOf('/') + 1) ?? MixDefaultValues.DefaultTemplateName;
            this.View = Templates.FirstOrDefault(t => !string.IsNullOrEmpty(templateName) && templateName.Equals($"{t.FileName}{t.Extension}"));
            if (this.View == null)
            {
                this.View = Templates.FirstOrDefault(t => MixDefaultValues.DefaultTemplateName.Equals($"{t.FileName}{t.Extension}"));
            }
            this.Template = $"{View?.FileFolder}/{View?.FileName}{View.Extension}";
            // Load Attributes
            // Load master views
            this.Masters = MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == MixTemplateFolderType.Masters, _context, _transaction).Data;
            var masterName = Layout?.Substring(Layout.LastIndexOf('/') + 1) ?? MixDefaultValues.DefaultTemplateName;
            this.Master = Masters.FirstOrDefault(t => !string.IsNullOrEmpty(masterName) && masterName.Equals($"{t.FileName}{t.Extension}"));
            if (this.Master == null)
            {
                this.Master = Masters.FirstOrDefault(t => MixDefaultValues.DefaultTemplateName.Equals($"{t.FileName}{t.Extension}"));
            }
            this.Layout = $"{Master?.FileFolder}/{Master?.FileName}{Master?.Extension}";

            this.ModuleNavs = GetModuleNavs(_context, _transaction);
            //this.ParentNavs = GetParentNavs(_context, _transaction);
            //this.ChildNavs = GetChildNavs(_context, _transaction);
            this.UrlAliases = GetAliases(_context, _transaction);
        }

        #region Sync

        public override RepositoryResponse<bool> SaveSubModels(MixPage parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };
            if (View.Id == 0)
            {
                var saveTemplate = View.SaveModel(true, _context, _transaction);
                ViewModelHelper.HandleResult(saveTemplate, ref result);
            }
            if (result.IsSucceed && Master != null)
            {
                var saveLayout = Master.SaveModel(true, _context, _transaction);
                ViewModelHelper.HandleResult(saveLayout, ref result);
            }
            if (result.IsSucceed && UrlAliases != null)
            {
                foreach (var item in UrlAliases)
                {
                    if (result.IsSucceed)
                    {
                        item.SourceId = parent.Id.ToString();
                        item.Type = MixUrlAliasType.Page;
                        item.Specificulture = Specificulture;
                        var saveResult = item.SaveModel(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (result.IsSucceed)
            {
                foreach (var item in ModuleNavs)
                {
                    item.PageId = parent.Id;

                    if (item.IsActived)
                    {
                        var saveResult = item.SaveModel(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        var saveResult = item.RemoveModel(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                }
            }
            return result;
        }

        #endregion Sync

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPage parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };

            if (View.Id == 0)
            {
                var saveTemplate = await View.SaveModelAsync(true, _context, _transaction);
                ViewModelHelper.HandleResult(saveTemplate, ref result);
            }

            if (result.IsSucceed && Master != null)
            {
                var saveLayout = Master.SaveModel(true, _context, _transaction);
                ViewModelHelper.HandleResult(saveLayout, ref result);
            }
            if (result.IsSucceed && UrlAliases != null)
            {
                foreach (var item in UrlAliases)
                {
                    if (result.IsSucceed)
                    {
                        item.SourceId = parent.Id.ToString();
                        item.Type = MixUrlAliasType.Page;
                        item.Specificulture = Specificulture;
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (result.IsSucceed)
            {
                foreach (var item in ModuleNavs)
                {
                    item.PageId = parent.Id;

                    if (item.IsActived)
                    {
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        var saveResult = await item.RemoveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                }
            }
            return result;
        }

        #endregion Async

        #endregion Overrides

        #region Expands

        private void GenerateSEO()
        {
            if (string.IsNullOrEmpty(this.SeoName))
            {
                this.SeoName = SeoHelper.GetSEOString(this.Title);
            }
            int i = 1;
            string name = SeoName;
            while (Repository.CheckIsExists(a => a.SeoName == name && a.Specificulture == Specificulture && a.Id != Id))
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

        public List<MixUrlAliases.UpdateViewModel> GetAliases(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixUrlAliases.UpdateViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture
                        && p.SourceId == Id.ToString() && p.Type == MixUrlAliasType.Page, context, transaction);
            if (result.IsSucceed && result.Data != null)
            {
                return result.Data;
            }
            else
            {
                return new List<MixUrlAliases.UpdateViewModel>();
            }
        }

        public List<MixPageModules.ReadMvcViewModel> GetModuleNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            // Load Actived Modules
            var result = MixPageModules.ReadMvcViewModel.Repository.GetModelListBy(m => m.PageId == Id && m.Specificulture == Specificulture).Data;
            result.ForEach(nav =>
            {
                nav.IsActived = true;
            });
            var moduleids = result.Select(m => m.ModuleId);
            // Load inactived modules
            var otherModules = MixModules.ReadListItemViewModel.Repository.GetModelListBy(m => m.Specificulture == Specificulture && !moduleids.Any(r => r == m.Id)).Data;
            foreach (var item in otherModules)
            {
                result.Add(new MixPageModules.ReadMvcViewModel()
                {
                    Specificulture = Specificulture,
                    PageId = Id,
                    ModuleId = item.Id,
                    Image = item.ImageUrl,
                    Description = item.Title
                });
            }
            return result.OrderBy(m => m.Priority).ToList();
        }

        #endregion Expands
    }
}