using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Services
{
    public class InitCmsService
    {
        public InitCmsService()
        {
        }

        public static async Task InitRolesAsync(RoleManager<IdentityRole> _roleManager)
        {
            await CreateRoleIfNotExist(_roleManager, MixDefaultRoles.SuperAdmin);
            await CreateRoleIfNotExist(_roleManager, MixDefaultRoles.Admin);
            await CreateRoleIfNotExist(_roleManager, MixDefaultRoles.Guest);
        }

        private static async Task CreateRoleIfNotExist(RoleManager<IdentityRole> _roleManager, string role)
        {
            var isExist = await _roleManager.RoleExistsAsync(role);
            if (!isExist)
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = role
                });
            }
        }

        /// <summary>
        /// Step 1
        ///     - Init Culture
        ///     - Init System pages
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static async Task<RepositoryResponse<bool>> InitCms(string siteName, InitCulture culture)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>();
            MixCmsContext context = null;
            MixCmsAccountContext accountContext = null;
            MixCacheDbContext cacheContext = null;
            try
            {
                string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
                if (!string.IsNullOrEmpty(cnn))
                {
                    context = MixService.GetDbContext();
                    accountContext = MixService.GetAccountDbContext();
                    cacheContext = MixCacheService.GetCacheDbContext();
                    await context.Database.MigrateAsync();
                    await accountContext.Database.MigrateAsync();
                    await cacheContext.Database.MigrateAsync();
                    var countCulture = context.MixCulture.Count();
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
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
            finally
            {
                context?.Dispose();
                accountContext?.Dispose();
            }
        }

        public static async Task<RepositoryResponse<bool>> InitSiteData(string siteName, InitCulture culture)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>();
            MixCmsContext context = null;
            IDbContextTransaction transaction = null;
            bool isSucceed = true;
            try
            {
                if (!string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    context = MixService.GetDbContext();
                    transaction = context.Database.BeginTransaction();

                    var countCulture = context.MixCulture.Count();

                    /**
                     * Init Selected Language as default
                     */
                    isSucceed = InitCultures(culture, context, transaction);

                    /**
                     * Init System Configurations
                     */
                    if (isSucceed && context.MixConfiguration.Count() == 0)
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
        public static async Task<RepositoryResponse<bool>> InitConfigurationsAsync(string siteName, string specifiCulture, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            /* Init Configs */

            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var getConfigs = MixFileRepository.Instance.GetFile(
                MixConstants.CONST_FILE_CONFIGURATIONS, MixFolders.DataFolder, true, "{}");
            var obj = JObject.Parse(getConfigs.Content);
            var configurations = obj["data"].ToObject<List<MixConfiguration>>();
            var cnfSiteName = configurations.Find(c => c.Keyword == MixAppSettingKeywords.SiteName);
            cnfSiteName.Value = siteName;
            if (!string.IsNullOrEmpty(cnfSiteName.Value))
            {
                configurations.Find(c => c.Keyword == MixAppSettingKeywords.ThemeName).Value = Common.Helper.SeoHelper.GetSEOString(cnfSiteName.Value);
                configurations.Find(c => c.Keyword == MixAppSettingKeywords.ThemeFolder).Value = Common.Helper.SeoHelper.GetSEOString(cnfSiteName.Value);
            }
            var result = await ViewModels.MixConfigurations.UpdateViewModel.ImportConfigurations(configurations, specifiCulture, context, transaction);

            UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);

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
        public static async Task<RepositoryResponse<bool>> InitMixDatabasesAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            /* Init Configs */

            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var getData = MixFileRepository.Instance.GetFile(MixConstants.CONST_FILE_ATTRIBUTE_SETS, MixFolders.DataFolder, true, "{}");
            var obj = JObject.Parse(getData.Content);
            var data = obj["data"].ToObject<List<ViewModels.MixDatabases.UpdateViewModel>>();
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

            UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);

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
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            /* Init Languages */
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);

            var result = await ViewModels.MixLanguages.UpdateViewModel.ImportLanguages(languages, specificulture, context, transaction);

            UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
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
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (!context.MixTheme.Any())
            {
                ViewModels.MixThemes.InitViewModel theme = new ViewModels.MixThemes.InitViewModel()
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
            UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            return new RepositoryResponse<bool>() { IsSucceed = result.IsSucceed };
        }

        protected static bool InitCultures(InitCulture culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            bool isSucceed = true;
            try
            {
                if (context.MixCulture.Count() == 0)
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

        protected static void InitPages(string culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            /* Init Pages */
            var pages = MixFileRepository.Instance.GetFile(MixConstants.CONST_FILE_PAGES, MixFolders.DataFolder, true, "{}");
            var obj = JObject.Parse(pages.Content);
            var arrPage = obj["data"].ToObject<List<MixPage>>();
            foreach (var page in arrPage)
            {
                page.Specificulture = culture;
                page.SeoTitle = page.Title.ToLower();
                page.SeoName = SeoHelper.GetSEOString(page.Title);
                page.SeoDescription = page.Title.ToLower();
                page.SeoKeywords = page.Title.ToLower();
                page.CreatedDateTime = DateTime.UtcNow;
                page.CreatedBy = MixDefaultRoles.SuperAdmin;
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