using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.Common;
using Mix.Domain.Core.ViewModels;
using Mix.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCommonController : ControllerBase
    {
        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("sitemap")]
        public async System.Threading.Tasks.Task<RepositoryResponse<FileViewModel>> SiteMapAsync()
        {
            try
            {
                return new RepositoryResponse<FileViewModel>();
                //XNamespace aw = "http://www.sitemaps.org/schemas/sitemap/0.9";
                //var root = new XElement(aw + "urlset");
                //var navigations = await Lib.ViewModels.MixAttributeSetDatas.ReadMvcViewModel.Repository.GetModelListByAsync(
                //        m => m.AttributeSetName == MixDatabaseNames.NAVIGATION
                //    );
                //var cultures = await Lib.ViewModels.MixCultures.ReadViewModel.Repository.GetModelListAsync();
                //List<int> handledPageId = new List<int>();
                

                //var posts = Lib.ViewModels.MixPosts.ReadListItemViewModel.Repository.GetModelList();
                //foreach (var post in posts.Data)
                //{
                //    var otherLanguages = navigations.Data.Where(p => p.Id == post.Id && p.Specificulture != post.Specificulture);
                //    var lstOther = new List<SitemapLanguage>();
                //    foreach (var item in otherLanguages)
                //    {
                //        lstOther.Add(new SitemapLanguage()
                //        {
                //            HrefLang = item.Specificulture,
                //            Href = MixCmsHelper.GetRouterUrl(
                //                        new { culture = item.Specificulture, seoName = post.SeoName }, Request, Url)
                //        });
                //    }
                //    var sitemap = new SiteMap()
                //    {
                //        ChangeFreq = "monthly",
                //        LastMod = DateTime.UtcNow,
                //        Loc = post.DetailsUrl,
                //        OtherLanguages = lstOther,
                //        Priority = 0.3
                //    };
                //    root.Add(sitemap.ParseXElement());
                //}

                //string folder = $"Sitemaps";
                //FileRepository.Instance.CreateDirectoryIfNotExist(folder);
                //string filename = $"sitemap";
                //string filePath = $"wwwroot/{folder}/{filename}.xml";
                //root.Save(filePath);
                //return new RepositoryResponse<FileViewModel>()
                //{
                //    IsSucceed = true,
                //    Data = new FileViewModel()
                //    {
                //        Extension = ".xml",
                //        Filename = filename,
                //        FileFolder = folder
                //    }
                //};
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<FileViewModel>() { Exception = ex };
            }
        }
    }
}
