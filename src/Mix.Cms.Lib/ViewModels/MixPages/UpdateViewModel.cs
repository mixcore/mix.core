using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
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
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPages
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixPage, UpdateViewModel>
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

        [JsonProperty("status")]
        public MixEnums.PageStatus Status { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("updatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("staticUrl")]
        public string StaticUrl { get; set; }

        [JsonProperty("level")]
        public int? Level { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("details")]
        public string DetailsUrl { get; set; }

        [JsonProperty("moduleNavs")]
        public List<MixPageModules.ReadMvcViewModel> ModuleNavs { get; set; } // Parent to Modules

        [JsonProperty("positionNavs")]
        public List<MixPagePositions.ReadViewModel> PositionNavs { get; set; } // Parent to Modules

        [JsonProperty("parentNavs")]
        public List<MixPagePages.ReadViewModel> ParentNavs { get; set; } // Parent to  Parent

        [JsonProperty("childNavs")]
        public List<MixPagePages.ReadViewModel> ChildNavs { get; set; } // Parent to  Parent

        [JsonProperty("listTag")]
        public JArray ListTag { get; set; } = new JArray();

        [JsonProperty("imageFileStream")]
        public FileStreamViewModel ImageFileStream { get; set; }

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
                    return Thumbnail;
                }
            }
        }

        #region Template

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonProperty("templates")]
        public List<MixTemplates.UpdateViewModel> Templates { get; set; }// Article Templates

        [JsonIgnore]
        public string ActivedTheme
        {
            get
            {
                return MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, Specificulture) ?? MixService.GetConfig<string>("DefaultTemplateFolder");
            }
        }

        [JsonIgnore]
        public string TemplateFolderType
        {
            get
            {
                return MixEnums.EnumTemplateFolder.Pages.ToString();
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
                    , ActivedTheme
                    , TemplateFolderType
                }
            );
            }
        }

        #endregion Template

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

            var navParent = ParentNavs?.FirstOrDefault(p => p.IsActived);

            if (navParent != null)
            {
                Level = Repository.GetSingleModel(c => c.Id == navParent.ParentId, _context, _transaction).Data.Level + 1;
            }
            else
            {
                Level = 0;
            }

            Template = View != null ? string.Format(@"{0}/{1}{2}", View.FolderType, View.FileName, View.Extension) : Template;
            if (Id == 0)
            {
                Id = Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            if (!string.IsNullOrEmpty(this.Tags))
            {
                ListTag = JArray.Parse(this.Tags);
            }

            this.Templates = this.Templates ?? MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Name == ActivedTheme && t.FolderType == this.TemplateFolderType).Data;
            this.View = MixTemplates.UpdateViewModel.GetTemplateByPath(Template, Specificulture, MixEnums.EnumTemplateFolder.Pages, _context, _transaction);
            this.Template = CommonHelper.GetFullPath(new string[]
               {
                    this.View?.FileFolder
                    , this.View?.FileName
               });

            this.ModuleNavs = GetModuleNavs(_context, _transaction);
            this.ParentNavs = GetParentNavs(_context, _transaction);
            this.ChildNavs = GetChildNavs(_context, _transaction);
            this.PositionNavs = GetPositionNavs(_context, _transaction);
        }

        #region Sync

        public override RepositoryResponse<bool> SaveSubModels(MixPage parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };
            var saveTemplate = View.SaveModel(true, _context, _transaction);
            result.IsSucceed = result.IsSucceed && saveTemplate.IsSucceed;
            if (saveTemplate.IsSucceed)
            {
                result.Errors.AddRange(saveTemplate.Errors);
                result.Exception = saveTemplate.Exception;
            }

            if (result.IsSucceed)
            {
                foreach (var item in ModuleNavs)
                {
                    item.CategoryId = parent.Id;
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
                foreach (var item in PositionNavs)
                {
                    item.CategoryId = parent.Id;
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
                foreach (var item in ParentNavs)
                {
                    item.Id = parent.Id;
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
                foreach (var item in ChildNavs)
                {
                    item.ParentId = parent.Id;
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

        #endregion Sync

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPage parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };
            var saveTemplate = await View.SaveModelAsync(true, _context, _transaction);
            result.IsSucceed = result.IsSucceed && saveTemplate.IsSucceed;
            if (saveTemplate.IsSucceed)
            {
                result.Errors.AddRange(saveTemplate.Errors);
                result.Exception = saveTemplate.Exception;
            }

            if (result.IsSucceed)
            {
                foreach (var item in ModuleNavs)
                {
                    item.CategoryId = parent.Id;

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
                foreach (var item in PositionNavs)
                {
                    item.CategoryId = parent.Id;
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
                foreach (var item in ParentNavs)
                {
                    item.Id = parent.Id;
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
                foreach (var item in ChildNavs)
                {
                    item.ParentId = parent.Id;
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

        #endregion Async

        #endregion Overrides

        #region Expands
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
                            IsSupported = culture.Specificulture == initCulture || _context.MixPage.Any(p => p.Id == Id && p.Specificulture == culture.Specificulture)
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

        public List<MixPagePositions.ReadViewModel> GetPositionNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            var query = context.MixPosition
                  .Include(cp => cp.MixPagePosition)
                  //.Where(p => p.Specificulture == Specificulture)
                  .Select(p => new MixPagePositions.ReadViewModel()
                  {
                      CategoryId = Id,
                      PositionId = p.Id,
                      Specificulture = Specificulture,
                      Description = p.Description,
                      IsActived = context.MixPagePosition.Count(m => m.CategoryId == Id && m.PositionId == p.Id && m.Specificulture == Specificulture) > 0
                  });

            return query.OrderBy(m => m.Priority).ToList();
        }

        public List<MixPageModules.ReadMvcViewModel> GetModuleNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            var query = context.MixModule
                .Include(cp => cp.MixPageModule)
                .Where(module => module.Specificulture == Specificulture)
                .Select(module => new MixPageModules.ReadMvcViewModel()
                {
                    CategoryId = Id,
                    ModuleId = module.Id,
                    Specificulture = Specificulture,
                    Description = module.Title,
                    Image = module.Image
                });

            var result = query.ToList();
            result.ForEach(nav =>
            {
                var currentNav = context.MixPageModule.FirstOrDefault(
                        m => m.ModuleId == nav.ModuleId && m.CategoryId == Id && m.Specificulture == Specificulture);
                nav.Priority = currentNav?.Priority ?? 0;
                nav.IsActived = currentNav != null;
            });
            return result.OrderBy(m => m.Priority).ToList();
        }

        public List<MixPagePages.ReadViewModel> GetParentNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            var query = context.MixPage
                .Include(cp => cp.MixPagePageMixPage)
                .Where(Category => Category.Specificulture == Specificulture && Category.Id != Id)
                .Select(Category =>
                    new MixPagePages.ReadViewModel()
                    {
                        Id = Id,
                        ParentId = Category.Id,
                        Specificulture = Specificulture,
                        Description = Category.Title,
                    }
                );

            var result = query.ToList();
            result.ForEach(nav =>
            {
                nav.IsActived = context.MixPagePage.Any(
                        m => m.ParentId == nav.ParentId && m.Id == Id && m.Specificulture == Specificulture);
            });
            return result.OrderBy(m => m.Priority).ToList();
        }

        public List<MixPagePages.ReadViewModel> GetChildNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            var query = context.MixPage
                .Include(cp => cp.MixPagePageMixPage)
                .Where(Category => Category.Specificulture == Specificulture && Category.Id != Id)
                .Select(Category =>
                new MixPagePages.ReadViewModel(
                      new MixPagePage()
                      {
                          Id = Category.Id,
                          ParentId = Id,
                          Specificulture = Specificulture,
                          Description = Category.Title,
                      }, context, transaction));

            var result = query.ToList();
            result.ForEach(nav =>
            {
                var currentNav = context.MixPagePage.FirstOrDefault(
                        m => m.ParentId == Id && m.Id == nav.Id && m.Specificulture == Specificulture);
                nav.Priority = currentNav?.Priority ?? 0;
                nav.IsActived = currentNav != null;
            });
            return result.OrderBy(m => m.Priority).ToList();
        }

        #endregion Expands
    }
}
