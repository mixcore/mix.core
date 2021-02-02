using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.Common;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Services;
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
                var getNavigations = await ViewModels.MixAttributeSetDatas.ReadMvcViewModel.Repository.GetModelListByAsync(
                    m => m.AttributeSetName == MixDatabaseNames.NAVIGATION,
                    context, transaction
                );
                XNamespace aw = "http://www.sitemaps.org/schemas/sitemap/0.9";
                var root = new XElement(aw + "urlset");
                List<int> handledPageId = new List<int>();
                var navs = getNavigations.Data.Select(n => n.Obj.ToObject<MixNavigation>());
                foreach (var nav in navs)
                {
                    ParseMenuItems(root, nav.MenuItems, context, transaction);
                }

                string folder = $"wwwroot";
                FileRepository.Instance.CreateDirectoryIfNotExist(folder);
                string filename = $"sitemap";
                string filePath = $"{folder}/{filename}.xml";
                root.Save(filePath);
                return new RepositoryResponse<FileViewModel>()
                {
                    IsSucceed = true,
                    Data = new FileViewModel()
                    {
                        Extension = ".xml",
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

        private static void ParseMenuItems(XElement root, List<MenuItem> menuItems, MixCmsContext context, IDbContextTransaction transaction)
        {
            foreach (var page in menuItems)
            {
                //var otherLanguages = menuItems.Where(p => p.Id == page.Id && p.Specificulture != page.Specificulture);
                var lstOther = new List<SitemapLanguage>();
                //foreach (var item in otherLanguages)
                //{
                //    lstOther.Add(new SitemapLanguage()
                //    {
                //        HrefLang = item.Specificulture,
                //        Href = MixCmsHelper.GetRouterUrl(
                //                   new { culture = item.Specificulture, seoName = page.SeoName }, Request, Url)
                //    });
                //}

                var sitemap = new SiteMap()
                {
                    ChangeFreq = "monthly",
                    LastMod = DateTime.UtcNow,
                    Loc = page.Href,
                    Priority = 0.3,
                    OtherLanguages = lstOther
                };
                var el = sitemap.ParseXElement();
                if (page.MenuItems.Count > 0)
                {
                    ParseMenuItems(root, page.MenuItems, context, transaction);
                }
                root.Add(el);
            }
        }
    }
}
