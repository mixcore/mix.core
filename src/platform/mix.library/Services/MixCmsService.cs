using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Database.Entities.MixDb;
using Mix.Identity.ViewModels;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;
using Mix.Mixdb.ViewModels;
using System.Xml.Linq;

namespace Mix.Lib.Services
{
    public class MixCmsService : TenantServiceBase, IMixCmsService
    {
        protected readonly UnitOfWorkInfo<MixDbDbContext> _mixdbUow;
        protected readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        protected readonly MixConfigurationService _configService;
        public MixCmsService(
            IHttpContextAccessor httpContextAccessor,
            MixConfigurationService configService,
            MixCacheService cacheService,
            UnitOfWorkInfo<MixDbDbContext> mixdbUow,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _configService = configService;
            _mixdbUow = mixdbUow;
            _cmsUow = cmsUow;
        }

        public string GetAssetFolder(string culture, string domain)
        {
            return $"{domain}/" +
                $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{_configService.GetConfig<string>(MixConfigurationNames.ThemeFolder, culture, CurrentTenant.Id)}";
        }

        public MixTenantSystemModel GetCurrentTenant()
        {
            return CurrentTenant;
        }

        public async Task<FileModel> ParseSitemapAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                XNamespace aw = "http://www.sitemaps.org/schemas/sitemap/0.9";
                var root = new XElement(aw + "urlset");

                await ParseNavigationsAsync(root, cancellationToken);
                await ParsePostsDocAsync(root);

                string folder = $"wwwroot";
                MixFileHelper.CreateFolderIfNotExist(folder);
                string filename = $"sitemap";
                string filePath = $"{folder}/{filename}{MixFileExtensions.Xml}";
                root.Save(filePath);
                return new FileModel()
                {
                    Extension = MixFileExtensions.Xml,
                    Filename = filename,
                    FileFolder = folder
                };
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        #region Navigation

        protected virtual async Task ParseNavigationsAsync(XElement root, CancellationToken cancellation = default)
        {
            var navs = await MixNavigationViewModel.GetRepository(_mixdbUow, CacheService).GetListAsync(
                m => m.TenantId == CurrentTenant.Id);

            foreach (var nav in navs)
            {
                ParseMenuItemsDoc(root, nav.MenuItems);
            }
        }

        protected virtual async Task ParsePostsDocAsync(XElement root, CancellationToken cancellation = default)
        {
            var posts = await MixPostViewModel.GetRepository(_cmsUow, CacheService).GetListAsync(m => m.TenantId == CurrentTenant.Id, cancellation);
            foreach (var post in posts)
            {
                ParsePostDoc(root, post);
            }
        }

        protected virtual void ParsePostDoc(XElement root, MixPostViewModel post)
        {
            var lstOther = new List<SitemapLanguage>();
            var sitemap = new SiteMap()
            {
                ChangeFreq = "monthly",
                LastMod = DateTime.UtcNow,
                Loc = GetFullUrl(post.Contents[0].DetailUrl),
                Priority = 0.3,
                OtherLanguages = lstOther
            };
            var el = sitemap.ParseXElement();

            if (post.Contents.Count > 1)
            {
                foreach (var other in post.Contents.Skip(1))
                {
                    lstOther.Add(new SitemapLanguage()
                    {
                        HrefLang = other.Specificulture,
                        Href = GetFullUrl(other.DetailUrl)
                    });
                }

            }


            root.Add(el);
        }

        protected virtual string GetFullUrl(string detailUrl)
        {
            if (detailUrl.IndexOf("http") < 0)
            {
                return $"https://{CurrentTenant.PrimaryDomain}/{detailUrl.TrimStart('/')}" ?? CurrentTenant.PrimaryDomain;
            }
            return detailUrl;
        }

        protected virtual void ParseMenuItemsDoc(XElement root, List<MixMenuItemViewModel> menuItems)
        {
            var dicMenuItems = menuItems
                .GroupBy(m => m.Id)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var item in dicMenuItems)
            {
                var page = item.Value.First();
                var lstOther = new List<SitemapLanguage>();
                foreach (var menu in item.Value.Skip(1))
                {
                    lstOther.Add(new SitemapLanguage()
                    {
                        HrefLang = menu.Hreflang,
                        Href = GetFullUrl(menu.Url)
                    });
                }

                var sitemap = new SiteMap()
                {
                    ChangeFreq = "monthly",
                    LastMod = DateTime.UtcNow,
                    Loc = GetFullUrl(page.Url),
                    Priority = 0.3,
                    OtherLanguages = lstOther
                };
                var el = sitemap.ParseXElement();

                root.Add(el);
            }
        }
        #endregion Navigation

    }
}