using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixCultures
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixCulture, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("lcid")]
        public string Lcid { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("configurations")]
        public List<MixConfigurations.ReadMvcViewModel> Configurations { get; set; }

        #endregion
        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixCulture model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides
        public override MixCulture ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getConfigurations = MixConfigurations.ReadMvcViewModel.Repository.GetModelListBy(c => c.Specificulture == Specificulture, _context, _transaction);
            if (getConfigurations.IsSucceed)
            {
                Configurations = getConfigurations.Data;
            }
        }
        #region Async
        public override async Task<RepositoryResponse<UpdateViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await base.SaveModelAsync(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed)
            {
                MixService.LoadFromDatabase();
                MixService.Save();
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixCulture parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var getPages = await MixPages.ReadViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
            if (getPages.IsSucceed)
            {
                foreach (var p in getPages.Data)
                {
                    p.Specificulture = Specificulture;
                    p.CreatedDateTime = DateTime.UtcNow;
                    p.LastModified = DateTime.UtcNow;
                    var saveResult = await p.SaveModelAsync(false, _context, _transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!saveResult.IsSucceed)
                    {
                        result.Errors.Add("Error: Clone Pages");
                        result.Errors.AddRange(saveResult.Errors);
                        result.Exception = saveResult.Exception;
                        break;
                    }
                }
            }
            if (result.IsSucceed)
            {
                var getConfigurations = await MixConfigurations.ReadMvcViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getConfigurations.IsSucceed)
                {
                    foreach (var c in getConfigurations.Data)
                    {
                        c.Specificulture = Specificulture;
                        c.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Configurations");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }

            }
            if (result.IsSucceed)
            {
                var getLanguages = await MixLanguages.ReadMvcViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getLanguages.IsSucceed)
                {
                    foreach (var c in getLanguages.Data)
                    {
                        c.Specificulture = Specificulture;
                        c.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Languages");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }
            
            if (result.IsSucceed)
            {
                var getMedias = await MixMedias.UpdateViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getMedias.IsSucceed)
                {
                    foreach (var c in getMedias.Data)
                    {
                        c.Specificulture = Specificulture;
                        c.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Medias");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }
            if (result.IsSucceed)
            {
                // Clone Module from Default culture
                var getModules = await MixModules.ReadListItemViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getModules.IsSucceed)
                {
                    foreach (var c in getModules.Data)
                    {
                        c.Specificulture = Specificulture;
                        c.CreatedDateTime = DateTime.UtcNow;
                        c.LastModified = DateTime.UtcNow;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Module");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }

            // Clone ModuleData from Default culture
            if (result.IsSucceed)
            {
                var getModuleDatas = await MixModuleDatas.ReadViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getModuleDatas.IsSucceed)
                {
                    foreach (var c in getModuleDatas.Data)
                    {
                        c.Specificulture = Specificulture;
                        c.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Module Data");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }
            // Clone Article from Default culture
            if (result.IsSucceed)
            {
                var getArticles = await MixArticles.ReadListItemViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getArticles.IsSucceed)
                {
                    foreach (var c in getArticles.Data)
                    {
                        c.Specificulture = Specificulture;
                        c.CreatedDateTime = DateTime.UtcNow;
                        c.LastModified = DateTime.UtcNow;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Articles");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }
            if (result.IsSucceed)
            {
                // Clone PageModule from Default culture
                var getPageModules = await MixPageModules.ReadMvcViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getPageModules.IsSucceed)
                {
                    foreach (var c in getPageModules.Data)
                    {
                        c.Specificulture = Specificulture;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Page Module");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }

            if (result.IsSucceed)
            {
                // Clone PagePosition from Default culture
                var getPagePositions = await MixPagePositions.ReadListItemViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getPagePositions.IsSucceed)
                {
                    foreach (var c in getPagePositions.Data)
                    {
                        c.Specificulture = Specificulture;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Page Position");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }

            // Clone PageArticle from Default culture
            if (result.IsSucceed)
            {
                var getPageArticles = await MixPageArticles.ReadViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getPageArticles.IsSucceed)
                {
                    foreach (var c in getPageArticles.Data)
                    {
                        c.Specificulture = Specificulture;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Page Article");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }
            // Clone ModuleArticle from Default culture
            if (result.IsSucceed)
            {

                var getModuleArticles = await MixModuleArticles.ReadViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getModuleArticles.IsSucceed)
                {
                    foreach (var c in getModuleArticles.Data)
                    {
                        c.Specificulture = Specificulture;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Module Article");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }
            // Clone ArticleArticle from Default culture
            if (result.IsSucceed)
            {
                var getArticleArticles = await MixArticleArticles.ReadViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getArticleArticles.IsSucceed)
                {
                    foreach (var c in getArticleArticles.Data)
                    {
                        c.Specificulture = Specificulture;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Article Article");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }

            // Clone ArticleMedia from Default culture
            if (result.IsSucceed)
            {
                var getArticleMedias = await MixArticleMedias.ReadViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getArticleMedias.IsSucceed)
                {
                    foreach (var c in getArticleMedias.Data)
                    {
                        c.Specificulture = Specificulture;
                        var saveResult = await c.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.Add("Error: Clone Article Media");
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            var configs = await _context.MixConfiguration.Where(c => c.Specificulture == Specificulture).ToListAsync();
            configs.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var languages = await _context.MixLanguage.Where(l => l.Specificulture == Specificulture).ToListAsync();
            languages.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var PageModules = await _context.MixPageModule.Where(l => l.Specificulture == Specificulture).ToListAsync();
            PageModules.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var PagePositions = await _context.MixPagePosition.Where(l => l.Specificulture == Specificulture).ToListAsync();
            PagePositions.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var PageArticles = await _context.MixPageArticle.Where(l => l.Specificulture == Specificulture).ToListAsync();
            PageArticles.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var ModuleArticles = await _context.MixModuleArticle.Where(l => l.Specificulture == Specificulture).ToListAsync();
            ModuleArticles.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var ArticleMedias = await _context.MixArticleMedia.Where(l => l.Specificulture == Specificulture).ToListAsync();
            ArticleMedias.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var ModuleDatas = await _context.MixModuleData.Where(l => l.Specificulture == Specificulture).ToListAsync();
            ModuleDatas.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var ArticleArticles = await _context.MixRelatedArticle.Where(l => l.Specificulture == Specificulture).ToListAsync();
            ArticleArticles.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var medias = await _context.MixMedia.Where(c => c.Specificulture == Specificulture).ToListAsync();
            medias.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
            
            var cates = await _context.MixPage.Where(c => c.Specificulture == Specificulture).ToListAsync();
            cates.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var modules = await _context.MixModule.Where(c => c.Specificulture == Specificulture).ToListAsync();
            modules.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var articles = await _context.MixArticle.Where(c => c.Specificulture == Specificulture).ToListAsync();
            articles.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var products = await _context.MixProduct.Where(c => c.Specificulture == Specificulture).ToListAsync();
            products.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
            result.IsSucceed = (await _context.SaveChangesAsync() > 0);
            return result;
        }

        public override async Task<RepositoryResponse<MixCulture>> RemoveModelAsync(bool isRemoveRelatedModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await base.RemoveModelAsync(isRemoveRelatedModels, _context, _transaction);
            if (result.IsSucceed)
            {
                if (result.IsSucceed)
                {
                    MixService.LoadFromDatabase();
                    MixService.Save();
                }
            }
            return result;
        }

        #endregion

        #endregion Overrides
    }
}
