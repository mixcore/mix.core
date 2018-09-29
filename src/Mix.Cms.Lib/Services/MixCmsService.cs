using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.Models.Account;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;
using MixCore.Cms.Lib.ViewModels.Init;
using Mix.Cms.Lib.Repositories;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Services
{
    public class InitCmsService
    {
        public InitCmsService()
        {
        }

        public async Task<RepositoryResponse<bool>> InitCms(InitCulture culture)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>();
            MixCmsContext context = null;
            MixCmsAccountContext accountContext = null;
            IDbContextTransaction transaction = null;
            IDbContextTransaction accTransaction = null;
            bool isSucceed = true;
            try
            {
                if (!string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    context = new MixCmsContext();
                    accountContext = new MixCmsAccountContext();
                    await context.Database.MigrateAsync();
                    await accountContext.Database.MigrateAsync();
                    transaction = context.Database.BeginTransaction();

                    var countCulture = context.MixCulture.Count();

                    var isInit = countCulture > 0;

                    if (!isInit)
                    {
                        isSucceed = InitCultures(culture, context, transaction);

                        isSucceed = isSucceed && InitPositions(context, transaction);

                        isSucceed = isSucceed && InitThemes(context, transaction);

                        isSucceed = isSucceed && await InitConfigurationsAsync(culture, context, transaction);
                        isSucceed = isSucceed && await InitLanguagesAsync(culture, context, transaction);
                    }
                    else
                    {
                        isSucceed = true;
                    }

                    if (isSucceed && context.MixCategory.Count()==0)
                    {
                        var cate = new MixCategory()
                        {
                            Id = 1,
                            Level = 0,
                            Title = "Home",
                            Specificulture = culture.Specificulture,
                            Template = "Pages/_Home.cshtml",
                            Type = (int)MixPageType.Home,
                            CreatedBy = "Admin",
                            CreatedDateTime = DateTime.UtcNow,
                            Status = (int)PageStatus.Published
                        };


                        context.Entry(cate).State = EntityState.Added;
                        var alias = new MixUrlAlias()
                        {
                            Id = 1,
                            SourceId = "1",
                            Type = (int)UrlAliasType.Page,
                            Specificulture = culture.Specificulture,
                            CreatedDateTime = DateTime.UtcNow,
                            Alias = cate.Title.ToLower()
                        };
                        context.Entry(alias).State = EntityState.Added;

                        var createVNHome = await context.SaveChangesAsync().ConfigureAwait(false);
                        isSucceed = createVNHome > 0;

                        var cate404 = new MixCategory()
                        {
                            Id = 2,
                            Title = "404",
                            Level = 0,
                            Specificulture = culture.Specificulture,
                            Template = "Pages/_404.cshtml",
                            Type = (int)MixPageType.Article,
                            CreatedBy = "Admin",
                            CreatedDateTime = DateTime.UtcNow,
                            Status = (int)PageStatus.Published
                        };

                        var alias404 = new MixUrlAlias()
                        {
                            Id = 2,
                            SourceId = "2",
                            Type = (int)UrlAliasType.Page,
                            Specificulture = culture.Specificulture,
                            CreatedDateTime = DateTime.UtcNow,
                            Alias = cate404.Title.ToLower()
                        };
                        context.Entry(cate404).State = EntityState.Added;
                        context.Entry(alias404).State = EntityState.Added;

                        var create404 = await context.SaveChangesAsync().ConfigureAwait(false);
                        isSucceed = create404 > 0;
                    }

                    if (isSucceed)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                result.IsSucceed = isSucceed;
                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                transaction?.Rollback();
                accTransaction?.Rollback();
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

        
        private async Task<bool> InitConfigurationsAsync(InitCulture culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            /* Init Configs */
            var configurations = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_CONFIGURATIONS, "data", true, "{}");
            var obj = JObject.Parse(configurations.Content);
            var arrConfiguration = obj["data"].ToObject<List<MixConfiguration>>();
            var result = await ViewModels.Configuration.ReadMvcViewModel.ImportConfigurations(arrConfiguration, culture.Specificulture,  context, transaction);
            return result.IsSucceed;

        }

        private async Task<bool> InitLanguagesAsync(InitCulture culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            /* Init Configs */
            var configurations = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_LANGUAGES, "data", true, "{}");
            var obj = JObject.Parse(configurations.Content);
            var arrLanguage = obj["data"].ToObject<List<MixLanguage>>();
            var result = await ViewModels.Language.ReadMvcViewModel.ImportLanguages(arrLanguage, culture.Specificulture,  context, transaction);
            return result.IsSucceed;

        }

        private bool InitThemes(MixCmsContext context, IDbContextTransaction transaction)
        {
            bool isSucceed = true;
            var getThemes = InitThemeViewModel.Repository.GetModelList(_context: context, _transaction: transaction);
            if (!context.MixTheme.Any())
            {
                InitThemeViewModel theme = new InitThemeViewModel(new MixTheme()
                {
                    Name = "Default",
                    CreatedBy = "Admin"
                }, context, transaction);

                isSucceed = isSucceed && theme.SaveModel(true, context, transaction).IsSucceed;
            }

            return isSucceed;
        }

        protected bool InitCultures(InitCulture culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            bool isSucceed = true;
            try
            {
                if (context.MixCulture.Count() == 0)
                {

                    // EN-US

                    var enCulture = new MixCulture()
                    {
                        Specificulture = culture.Specificulture,
                        FullName = culture.FullName,
                        Description = culture.Description,
                        Icon = culture.Icon,
                        Alias = culture.Alias
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

        protected bool InitPositions(MixCmsContext context, IDbContextTransaction transaction)
        {
            bool isSucceed = true;
            var count = context.MixPortalPage.Count();
            if (count == 0)
            {
                var p = new MixPosition()
                {
                    Description = nameof(MixEnums.CatePosition.Nav)
                };
                context.Entry(p).State = EntityState.Added;
                p = new MixPosition()
                {
                    Description = nameof(MixEnums.CatePosition.Top)
                };
                context.Entry(p).State = EntityState.Added;
                p = new MixPosition()
                {
                    Description = nameof(MixEnums.CatePosition.Left)
                };
                context.Entry(p).State = EntityState.Added;
                p = new MixPosition()
                {
                    Description = nameof(MixEnums.CatePosition.Footer)
                };
                context.Entry(p).State = EntityState.Added;

                context.SaveChanges();
            }
            return isSucceed;
        }

        //public void RefreshAll(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        //{
        //    RefreshConfigurations(_context, _transaction);
        //    RefreshCultures(_context, _transaction);
        //}

        //public void RefreshConfigurations(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        //{
        //    this.CmsConfigurations = new CmsConfiguration();
        //    if (!string.IsNullOrEmpty(CmsConfigurations.CmsConnectionString))
        //    {
        //        CmsConfigurations.Init(_context, _transaction);
        //    }
        //}

        //public void RefreshCultures(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        //{
        //    this.CmsCulture = new CmsCultureConfiguration();
        //    if (!string.IsNullOrEmpty(CmsConfigurations.CmsConnectionString))
        //    {
        //        CmsCulture.Init(_context, _transaction);
        //    }
        //}

        //public string GetLocalString(string key, string culture, string defaultValue = null)
        //{
        //    return this.CmsConfigurations.GetLocalString(key, culture, defaultValue);
        //}

        //public int GetLocalInt(string key, string culture, int defaultValue = 0)
        //{
        //    return this.CmsConfigurations.GetLocalInt(key, culture, defaultValue);
        //}

    }
}
