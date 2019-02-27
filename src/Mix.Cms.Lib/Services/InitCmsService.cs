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
using Mix.Cms.Lib.Repositories;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Mix.Cms.Messenger.Models.Data;
using Mix.Common.Helper;

namespace Mix.Cms.Lib.Services
{
    public class InitCmsService
    {
        public InitCmsService()
        {
        }

        public async Task<RepositoryResponse<bool>> InitCms(string siteName, InitCulture culture)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>();
            MixCmsContext context = null;
            MixCmsAccountContext accountContext = null;
            MixChatServiceContext messengerContext;
            IDbContextTransaction transaction = null;
            IDbContextTransaction accTransaction = null;
            bool isSucceed = true;
            try
            {
                if (!string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    context = new MixCmsContext();
                    accountContext = new MixCmsAccountContext();
                    messengerContext = new MixChatServiceContext();
                    //MixChatServiceContext._cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
                    await context.Database.MigrateAsync();
                    await accountContext.Database.MigrateAsync();
                    await messengerContext.Database.MigrateAsync();
                    transaction = context.Database.BeginTransaction();

                    var countCulture = context.MixCulture.Count();

                    var isInit = countCulture > 0;

                    if (!isInit)
                    {
                        MixService.SetConfig<string>("SiteName", siteName);
                        isSucceed = InitCultures(culture, context, transaction);
                        if (isSucceed)
                        {
                            isSucceed = isSucceed && InitPositions(context, transaction);
                        }
                        if (isSucceed)
                        {
                            isSucceed = isSucceed && await InitConfigurationsAsync(siteName, culture, context, transaction);
                        }
                        if (isSucceed)
                        {
                            isSucceed = isSucceed && await InitLanguagesAsync(culture, context, transaction);
                        }
                        if (isSucceed)
                        {
                            isSucceed = isSucceed && InitThemes(siteName, context, transaction);
                        }
                    }
                    else
                    {
                        isSucceed = true;
                    }

                    if (isSucceed && context.MixPage.Count() == 0)
                    {
                        var cate = new MixPage()
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

                        var cate404 = new MixPage()
                        {
                            Id = 2,
                            Title = "404",
                            SeoName = "404",
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

                        var cate403 = new MixPage()
                        {
                            Id = 2,
                            Title = "403",
                            SeoName = "403",
                            Level = 0,
                            Specificulture = culture.Specificulture,
                            Template = "Pages/_403.cshtml",
                            Type = (int)MixPageType.Article,
                            CreatedBy = "Admin",
                            CreatedDateTime = DateTime.UtcNow,
                            Status = (int)PageStatus.Published
                        };

                        var alias403 = new MixUrlAlias()
                        {
                            Id = 2,
                            SourceId = "2",
                            Type = (int)UrlAliasType.Page,
                            Specificulture = culture.Specificulture,
                            CreatedDateTime = DateTime.UtcNow,
                            Alias = cate403.Title.ToLower()
                        };
                        context.Entry(cate403).State = EntityState.Added;
                        context.Entry(alias403).State = EntityState.Added;

                        var create403 = await context.SaveChangesAsync().ConfigureAwait(false);
                        isSucceed = create403 > 0;
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


        private async Task<bool> InitConfigurationsAsync(string siteName, InitCulture culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            /* Init Configs */
            var configurations = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_CONFIGURATIONS, "data", true, "{}");
            var obj = JObject.Parse(configurations.Content);
            var arrConfiguration = obj["data"].ToObject<List<MixConfiguration>>();
            if (!string.IsNullOrEmpty(siteName))
            {
                arrConfiguration.Find(c => c.Keyword == "SiteName").Value = siteName;
                arrConfiguration.Find(c => c.Keyword == "ThemeName").Value = Common.Helper.SeoHelper.GetSEOString(siteName);
                arrConfiguration.Find(c => c.Keyword == "ThemeFolder").Value = Common.Helper.SeoHelper.GetSEOString(siteName);
            }
            var result = await ViewModels.MixConfigurations.ReadMvcViewModel.ImportConfigurations(arrConfiguration, culture.Specificulture, context, transaction);
            return result.IsSucceed;

        }

        private async Task<bool> InitLanguagesAsync(InitCulture culture, MixCmsContext context, IDbContextTransaction transaction)
        {
            /* Init Languages */
            var configurations = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_LANGUAGES, "data", true, "{}");
            var obj = JObject.Parse(configurations.Content);
            var arrLanguage = obj["data"].ToObject<List<MixLanguage>>();
            var result = await ViewModels.MixLanguages.ReadMvcViewModel.ImportLanguages(arrLanguage, culture.Specificulture, context, transaction);
            return result.IsSucceed;

        }

        private bool InitThemes(string siteName, MixCmsContext context, IDbContextTransaction transaction)
        {
            bool isSucceed = true;
            var getThemes = ViewModels.MixThemes.InitViewModel.Repository.GetModelList(_context: context, _transaction: transaction);
            if (!context.MixTheme.Any())
            {
                ViewModels.MixThemes.InitViewModel theme = new ViewModels.MixThemes.InitViewModel(new MixTheme()
                {
                    Id = 1,
                    Title = siteName,
                    Name = SeoHelper.GetSEOString(siteName),
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Status = (int)MixContentStatus.Published,
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
                        Id = 1,
                        Specificulture = culture.Specificulture,
                        FullName = culture.FullName,
                        Description = culture.Description,
                        Icon = culture.Icon,
                        Alias = culture.Alias,
                        Status = (int)MixEnums.MixContentStatus.Published,
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

        protected bool InitPositions(MixCmsContext context, IDbContextTransaction transaction)
        {
            bool isSucceed = true;
            var count = context.MixPortalPage.Count();
            if (count == 0)
            {
                var p = new MixPosition()
                {
                    Id = 1,
                    Description = nameof(MixEnums.CatePosition.Nav)
                };
                context.Entry(p).State = EntityState.Added;
                p = new MixPosition()
                {
                    Id = 2,
                    Description = nameof(MixEnums.CatePosition.Top)
                };
                context.Entry(p).State = EntityState.Added;
                p = new MixPosition()
                {
                    Id = 3,
                    Description = nameof(MixEnums.CatePosition.Left)
                };
                context.Entry(p).State = EntityState.Added;
                p = new MixPosition()
                {
                    Id = 4,
                    Description = nameof(MixEnums.CatePosition.Footer)
                };
                context.Entry(p).State = EntityState.Added;

                context.SaveChanges();
            }
            return isSucceed;
        }

    }
}
