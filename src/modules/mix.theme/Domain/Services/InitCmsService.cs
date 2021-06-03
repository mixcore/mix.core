using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Lib.Helpers;
using Mix.Services;
using Mix.Theme.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Shared.Services;
using Mix.Database.Services;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.Entities.Account;

namespace Mix.Lib.Services
{
    public class InitCmsService
    {
        public InitCmsService()
        {
        }

        /// <summary>
        /// Step 1
        ///     - Init Culture
        ///     - Init System pages
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static async Task<RepositoryResponse<bool>> InitCms(string siteName, InitCultureModel culture)
        {
            RepositoryResponse<bool> result = new();
            MixCmsContextV2 context = null;
            MixCmsAccountContext accountContext = null;
            try
            {
                string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
                if (!string.IsNullOrEmpty(cnn))
                {
                    context = MixDatabaseService.GetDbContext();
                    accountContext = MixDatabaseService.GetAccountDbContext();
                    MixCacheDbContext cacheContext = MixCacheService.GetCacheDbContext();
                    await context.Database.MigrateAsync();
                    await accountContext.Database.MigrateAsync();
                    await cacheContext.Database.MigrateAsync();
                    var countCulture = context.MixCultures.Count();
                    var pendingMigration = context.Database.GetPendingMigrations().Count();
                    if (pendingMigration == 0)
                    {
                        return await InitSiteData(siteName, culture);
                    }                    
                }
                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                MixHelper.HandleException(result, ex);
                return result;
            }
            finally
            {
                context?.Dispose();
                accountContext?.Dispose();
            }
        }

        public static async Task<RepositoryResponse<bool>> InitSiteData(string siteName, InitCultureModel culture)
        {
            RepositoryResponse<bool> result = new();
            MixCmsContextV2 context = null;
            IDbContextTransaction transaction = null;
            try
            {
                if (!string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    context = MixDatabaseService.GetDbContext();
                    transaction = context.Database.BeginTransaction();

                    var countCulture = context.MixCultures.Count();

                    /**
         * Init Selected Language as default
         */
                    bool isSucceed = InitCultures(culture, context);

                    /**
                     * Init System Configurations
                     */
                    if (isSucceed && !context.MixConfigurations.Any())
                    {
                        var saveResult = await InitConfigurationsAsync(siteName, culture.Specificulture, context, transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        result.Errors = saveResult.Errors;
                        result.Exception = saveResult.Exception;
                    }
                    else
                    {
                        result.IsSucceed = false;
                        result.Errors.Add("Cannot init cultures");
                    }
                    if (result.IsSucceed)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                transaction?.Rollback();
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
            finally
            {
                context?.Database.CloseConnection();
                context?.Dispose();
            }
        }

        /// <summary>
        /// Step 2
        ///     - Init Configurations
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="specifiCulture"></param>
        /// <param name="_context"></param>
        /// <param name="_transaction"></param>
        /// <returns></returns>
        public static async Task<RepositoryResponse<bool>> InitConfigurationsAsync(string siteName, string specifiCulture, MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            /* Init Configs */

            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(
                _context, _transaction, out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);
            var getConfigs = MixFileRepository.Instance.GetFile(
                MixConstants.CONST_FILE_CONFIGURATIONS, MixFolders.JsonDataFolder, true, "{}");
            var obj = JObject.Parse(getConfigs.Content);
            var configurations = obj["data"].ToObject<List<MixConfigurationContent>>();
            var cnfSiteName = configurations.Find(c => c.SystemName == MixAppSettingKeywords.SiteName);
            cnfSiteName.Content = siteName;
            if (!string.IsNullOrEmpty(cnfSiteName.Content))
            {
                configurations.Find(c => c.SystemName == MixAppSettingKeywords.ThemeName).Content = 
                    SeoHelper.GetSEOString(cnfSiteName.Content);
                configurations.Find(c => c.SystemName == MixAppSettingKeywords.ThemeFolder).Content = 
                    SeoHelper.GetSEOString(cnfSiteName.Content);
            }
            var result = await ThemeHelper.ImportConfigurations(configurations, specifiCulture, context, transaction);

            UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(result.IsSucceed, isRoot, transaction);

            return result;
        }

        /// <summary>
        /// Step 2
        ///     - Init Configurations
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="specifiCulture"></param>
        /// <param name="_context"></param>
        /// <param name="_transaction"></param>
        /// <returns></returns>
        public static async Task<RepositoryResponse<bool>> InitMixDatabasesAsync(MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            /* Init Configs */

            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(
                _context, _transaction, out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var getData = MixFileRepository.Instance.GetFile(
                MixConstants.CONST_FILE_ATTRIBUTE_SETS, MixFolders.JsonDataFolder, true, "{}");
            var obj = JObject.Parse(getData.Content);
            var data = obj["data"].ToObject<List<ImportMixDatabaseViewModel>>();
            foreach (var item in data)
            {
                if (result.IsSucceed)
                {
                    item.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(true, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    break;
                }
            }

            UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(result.IsSucceed, isRoot, transaction);

            return result;
        }

        /// <summary>
        /// Step 3
        ///     - Init Languages for translate
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="_context"></param>
        /// <param name="_transaction"></param>
        /// <returns></returns>
        public async Task<RepositoryResponse<bool>> InitLanguagesAsync(string specificulture, List<MixLanguage> languages
            , MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            /* Init Languages */
            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(_context, _transaction, out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);

            var result = await ThemeHelper.ImportLanguages(languages, specificulture, context, transaction);

            UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            return result;
        }

        /// <summary>
        /// Step 4
        ///     - Init default theme
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="_context"></param>
        /// <param name="_transaction"></param>
        /// <returns></returns>
        public async Task<RepositoryResponse<bool>> InitThemesAsync(string siteName
            , MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(_context, _transaction, out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (!context.MixTheme.Any())
            {
                var theme = new InitThemeViewModel()
                {
                    Id = 1,
                    Title = siteName,
                    Name = SeoHelper.GetSEOString(siteName),
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Status = MixContentStatus.Published,
                };
                theme.ExpandView(context, transaction);
                var saveResult = await theme.SaveModelAsync(true, context, transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
            }
            UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            return new RepositoryResponse<bool>() { IsSucceed = result.IsSucceed };
        }

        protected static bool InitCultures(InitCultureModel culture, MixCmsContextV2 context)
        {
            bool isSucceed = true;
            try
            {
                if (!context.MixCultures.Any())
                {
                    // EN-US

                    var enCulture = new MixCulture()
                    {
                        Id = 1,
                        Specificulture = culture.Specificulture,
                        FullName = culture.FullName,
                        Description = culture.Description,
                        Icon = culture.Icon,
                        Alias = culture.Alias,
                        Status = MixContentStatus.Published,
                        CreatedDateTime = DateTime.UtcNow
                    };
                    context.Entry(enCulture).State = EntityState.Added;

                    context.SaveChanges();
                }
            }
            catch
            {
                isSucceed = false;
            }
            return isSucceed;
        }

        protected static void InitPages(string culture, MixCmsContextV2 context)
        {
            /* Init Pages */
            var pages = MixFileRepository.Instance.GetFile(MixConstants.CONST_FILE_PAGES, MixFolders.JsonDataFolder, true, "{}");
            var obj = JObject.Parse(pages.Content);
            var arrPage = obj["data"].ToObject<List<MixPageContent>>();
            foreach (var page in arrPage)
            {
                page.Specificulture = culture;
                page.SeoTitle = page.Title.ToLower();
                page.SeoName = SeoHelper.GetSEOString(page.Title);
                page.SeoDescription = page.Title.ToLower();
                page.SeoKeywords = page.Title.ToLower();
                page.CreatedDateTime = DateTime.UtcNow;
                context.Entry(page).State = EntityState.Added;
                var alias = new MixUrlAlias()
                {
                    Id = page.Id,
                    SourceId = page.Id.ToString(),
                    Type = (int)MixUrlAliasType.Page,
                    Specificulture = culture,
                    CreatedDateTime = DateTime.UtcNow,
                    Alias = page.Title.ToLower(),
                    Status = MixContentStatus.Published
                };
                context.Entry(alias).State = EntityState.Added;
            }
        }
    }
}