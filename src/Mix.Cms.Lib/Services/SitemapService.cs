using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.Common;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mix.Cms.Lib.Services
{
    public class SitemapService
    {
        public static async Task<RepositoryResponse<FileViewModel>> ParseSitemapAsync()
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(null, null, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                XNamespace aw = "http://www.sitemaps.org/schemas/sitemap/0.9";
                var root = new XElement(aw + "urlset");

                await ParseNavigationsAsync(root, context, transaction);
                await ParsePostsDocAsync(root, context, transaction);

                string folder = $"wwwroot";
                MixFileRepository.Instance.CreateDirectoryIfNotExist(folder);
                string filename = $"sitemap";
                string filePath = $"{folder}/{filename}{MixFileExtensions.Xml}";
                root.Save(filePath);
                return new RepositoryResponse<FileViewModel>()
                {
                    IsSucceed = true,
                    Data = new FileViewModel()
                    {
                        Extension = MixFileExtensions.Xml,
                        Filename = filename,
                        FileFolder = folder
                    }
                };
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<FileViewModel>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        #region Post

        private static async Task ParsePostsDocAsync(XElement root, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getPosts = await Lib.ViewModels.MixPosts.ReadListItemViewModel.Repository.GetModelListAsync(context, transaction);
            var dicPosts = getPosts.Data
               .GroupBy(m => m.Id)
               .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var item in dicPosts)
            {
                var post = item.Value.First();
                var lstOther = new List<SitemapLanguage>();
                foreach (var menu in item.Value.Skip(1))
                {
                    lstOther.Add(new SitemapLanguage()
                    {
                        HrefLang = menu.Specificulture,
                        Href = menu.DetailsUrl
                    });
                }

                var sitemap = new SiteMap()
                {
                    ChangeFreq = "monthly",
                    LastMod = DateTime.UtcNow,
                    Loc = post.DetailsUrl,
                    Priority = 0.3,
                    OtherLanguages = lstOther
                };
                var el = sitemap.ParseXElement();

                root.Add(el);
            }
        }

        #endregion Post

        #region Navigation

        private static async Task ParseNavigationsAsync(XElement root, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getNavigations = await ViewModels.MixDatabaseDatas.ReadMvcViewModel.Repository.GetModelListByAsync(
                   m => m.MixDatabaseName == MixDatabaseNames.NAVIGATION,
                   context, transaction
               );
            var navs = getNavigations.Data.Select(n => new MixNavigation(n.Obj, n.Specificulture)).ToList();
            List<MenuItem> menuItems = new List<MenuItem>();
            navs.ForEach(n => menuItems.AddRange(n.MenuItems));
            var subMenus = ParseMenuItems(menuItems, context, transaction);
            menuItems.AddRange(subMenus);
            ParseMenuItemsDoc(root, menuItems, context, transaction);
        }

        private static List<MenuItem> ParseMenuItems(List<MenuItem> menuItems, MixCmsContext context, IDbContextTransaction transaction)
        {
            List<MenuItem> subMenuItems = new List<MenuItem>();

            foreach (var item in menuItems)
            {
                if (item.MenuItems.Count > 0)
                {
                    subMenuItems.AddRange(item.MenuItems.Where(m => !menuItems.Any(p => p.Href == m.Href)));
                    subMenuItems.ForEach(m => m.Specificulture = item.Specificulture);
                }
            }
            if (subMenuItems.Count > 0)
            {
                subMenuItems.AddRange(ParseMenuItems(subMenuItems, context, transaction));
            }
            return subMenuItems;
        }

        private static void ParseMenuItemsDoc(XElement root, List<MenuItem> menuItems, MixCmsContext context, IDbContextTransaction transaction)
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
                        HrefLang = menu.Specificulture,
                        Href = menu.Href
                    });
                }

                var sitemap = new SiteMap()
                {
                    ChangeFreq = "monthly",
                    LastMod = DateTime.UtcNow,
                    Loc = page.Href,
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