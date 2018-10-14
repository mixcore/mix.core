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
            if (Id==0)
            {
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
            if (Id == 0)
            {
                var getPages = await MixPages.ReadViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        var page = new MixPage()
                        {
                            Specificulture = Specificulture,
                            Id = p.Id,
                            Content = p.Content,
                            CreatedBy = p.CreatedBy,
                            CreatedDateTime = DateTime.UtcNow,
                            Layout = p.Layout,
                            CssClass = p.CssClass,
                            Excerpt = p.Excerpt,
                            Icon = p.Icon,
                            Image = p.Image,
                            Level = p.Level,
                            ModifiedBy = p.ModifiedBy,
                            PageSize = p.PageSize,
                            Priority = p.Priority,
                            SeoDescription = p.SeoDescription,
                            SeoKeywords = p.SeoKeywords,
                            SeoName = p.SeoName,
                            SeoTitle = p.SeoTitle,
                            StaticUrl = p.StaticUrl,
                            Status = (int)p.Status,
                            Tags = p.Tags,
                            Template = p.Template,
                            Title = p.Title,
                            Type = (int)p.Type,
                        };
                        _context.Entry(page).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                }
                var getConfigurations = await MixConfigurations.ReadMvcViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getConfigurations.IsSucceed)
                {
                    foreach (var c in getConfigurations.Data)
                    {
                        var cnf = new MixConfiguration()
                        {
                            Keyword = c.Keyword,
                            Specificulture = Specificulture,
                            Category = c.Category,
                            DataType = (int)c.DataType,
                            Description = c.Description,
                            Priority = c.Priority,
                            Status = (int)c.Status,
                            Value = c.Value
                        };
                        _context.Entry(cnf).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                }

                var getLanguages = await MixLanguages.ReadMvcViewModel.Repository.GetModelListByAsync(c => c.Specificulture == MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultCulture), _context, _transaction);
                if (getLanguages.IsSucceed)
                {
                    foreach (var c in getLanguages.Data)
                    {
                        var cnf = new MixLanguage()
                        {
                            Keyword = c.Keyword,
                            Specificulture = Specificulture,
                            Category = c.Category,
                            DataType = (int)c.DataType,
                            Description = c.Description,
                            Priority = c.Priority,
                            Status = (int)c.Status,
                            DefaultValue = c.DefaultValue
                        };
                        _context.Entry(cnf).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                }
                _context.SaveChanges();
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            var configs = _context.MixConfiguration.Where(c => c.Specificulture == Specificulture).ToList();
            configs.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var languages = _context.MixLanguage.Where(l => l.Specificulture == Specificulture).ToList();
            languages.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var cates = _context.MixPage.Where(c => c.Specificulture == Specificulture).ToList();
            cates.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var modules = _context.MixModule.Where(c => c.Specificulture == Specificulture).ToList();
            modules.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var articles = _context.MixArticle.Where(c => c.Specificulture == Specificulture).ToList();
            articles.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var products = _context.MixProduct.Where(c => c.Specificulture == Specificulture).ToList();
            products.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            await _context.SaveChangesAsync();

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
