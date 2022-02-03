using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixCultures
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixCulture, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

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

        [JsonProperty("configurations")]
        public List<MixConfigurations.ReadMvcViewModel> Configurations { get; set; }

        #endregion Views

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
                Id = Repository.Max(m => m.Id, _context, _transaction).Data + 1;
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
                MixService.SaveSettings();
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixCulture parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            // Clone Configurations
            if (result.IsSucceed)
            {
                var cloneResult = await CloneConfigurationsAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            if (result.IsSucceed)
            {
                var cloneResult = await CloneLanguagesAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            if (result.IsSucceed)
            {
                var cloneResult = await CloneMediasAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }
            if (result.IsSucceed)
            {
                var cloneResult = await CloneModulesAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            // Clone Pages
            if (result.IsSucceed)
            {
                var cloneResult = await ClonePagesAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            // Clone Post from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await ClonePostsAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }
            // Clone ModuleData from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await CloneModuleDatasAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }
            // Clone PageModules from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await ClonePageModulesAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            // Clone PagePost from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await ClonePagePostsAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            // Clone ModulePost from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await CloneModulePostsAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            // Clone ModulePost from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await CloneModuleDatasAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }
            // Clone PostPost from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await ClonePostPostsAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            // Clone PostMedia from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await ClonePostMediasAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            // Clone PostMedia from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await CloneUrlAliasAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            // Clone Attribute Value from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await CloneMixDatabaseDataValuesAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }
            // Clone Attribute Data from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await CloneAttributeDatasAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }
            // Clone Related Data from Default culture
            if (result.IsSucceed)
            {
                var cloneResult = await CloneRelatedAttributeDatasAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(cloneResult, ref result);
            }

            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneRelatedAttributeDatasAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixDatabaseDataAssociation>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixDatabaseDataAssociation.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            p.CreatedDateTime = DateTime.UtcNow;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneAttributeDatasAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixDatabaseData>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixDatabaseData.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            p.CreatedDateTime = DateTime.UtcNow;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneMixDatabaseDataValuesAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixDatabaseDataValue>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixDatabaseDataValue.Any(m => m.DataId == p.DataId && m.Specificulture == Specificulture))
                        {
                            p.Id = Guid.NewGuid().ToString();
                            if (!string.IsNullOrEmpty(p.StringValue) && p.StringValue.Contains($"/{p.Specificulture}"))
                            {
                                p.StringValue = p.StringValue.Replace($"/{p.Specificulture}", $"/{Specificulture}");
                            }
                            p.Specificulture = Specificulture;
                            p.CreatedDateTime = DateTime.UtcNow;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneUrlAliasAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixUrlAlias>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixUrlAlias.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            p.CreatedDateTime = DateTime.UtcNow;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneModulesAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixModule>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixModule.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneMediasAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixMedia>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixMedia.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> ClonePostMediasAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixPostMedia>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixPostMedia.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> ClonePostPostsAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixPostAssociation>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixRelatedPost.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            p.CreatedDateTime = DateTime.UtcNow;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneModulePostsAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getModules = await DefaultModelRepository<MixCmsContext, MixModulePost>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getModules.IsSucceed)
                {
                    foreach (var p in getModules.Data)
                    {
                        if (!context.MixModulePost.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> ClonePagePostsAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixPagePost>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixPagePost.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> ClonePageModulesAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixPageModule>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixPageModule.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> ClonePostsAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixPost>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixPost.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneModuleDatasAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixModuleData>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixModuleData.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneLanguagesAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixLanguage>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixLanguage.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            p.CreatedDateTime = DateTime.UtcNow;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> CloneConfigurationsAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixConfiguration>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixConfiguration.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            p.CreatedDateTime = DateTime.UtcNow;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        public async Task<RepositoryResponse<bool>> ClonePagesAsync(MixCulture parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                var getPages = await DefaultModelRepository<MixCmsContext, MixPage>.Instance.GetModelListByAsync(
                    c => c.Specificulture == MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture),
                    context, transaction);
                if (getPages.IsSucceed)
                {
                    foreach (var p in getPages.Data)
                    {
                        if (!context.MixPage.Any(m => m.Id == p.Id && m.Specificulture == Specificulture))
                        {
                            p.Specificulture = Specificulture;
                            p.CreatedDateTime = DateTime.UtcNow;
                            p.LastModified = DateTime.UtcNow;
                            context.Entry(p).State = EntityState.Added;
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                result.Errors.Add(ex.Message);
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

            var PagePosts = await _context.MixPagePost.Where(l => l.Specificulture == Specificulture).ToListAsync();
            PagePosts.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var ModulePosts = await _context.MixModulePost.Where(l => l.Specificulture == Specificulture).ToListAsync();
            ModulePosts.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var PostMedias = await _context.MixPostMedia.Where(l => l.Specificulture == Specificulture).ToListAsync();
            PostMedias.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var ModuleDatas = await _context.MixModuleData.Where(l => l.Specificulture == Specificulture).ToListAsync();
            ModuleDatas.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var PostPosts = await _context.MixRelatedPost.Where(l => l.Specificulture == Specificulture).ToListAsync();
            PostPosts.ForEach(l => _context.Entry(l).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var medias = await _context.MixMedia.Where(c => c.Specificulture == Specificulture).ToListAsync();
            medias.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var cates = await _context.MixPage.Where(c => c.Specificulture == Specificulture).ToListAsync();
            cates.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var modules = await _context.MixModule.Where(c => c.Specificulture == Specificulture).ToListAsync();
            modules.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var posts = await _context.MixPost.Where(c => c.Specificulture == Specificulture).ToListAsync();
            posts.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var aliases = await _context.MixUrlAlias.Where(c => c.Specificulture == Specificulture).ToListAsync();
            aliases.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var values = await _context.MixDatabaseDataValue.Where(c => c.Specificulture == Specificulture).ToListAsync();
            values.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var datas = await _context.MixDatabaseData.Where(c => c.Specificulture == Specificulture).ToListAsync();
            datas.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            var relateddatas = await _context.MixDatabaseDataAssociation.Where(c => c.Specificulture == Specificulture).ToListAsync();
            relateddatas.ForEach(c => _context.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

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
                    MixService.SaveSettings();
                }
            }
            return result;
        }

        #endregion Async

        #endregion Overrides
    }
}