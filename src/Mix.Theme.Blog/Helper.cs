using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MixDatas = Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using MixPosts = Mix.Cms.Lib.ViewModels.MixPosts;

namespace Mix.Theme.Blog
{
    public static class Helper
    {
        public static async Task<dynamic> GetPosts(string culture = null, int? pageSize = 15, int? pageIndex = 0)
        {
            culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            return await MixPosts.ReadMvcViewModel.Repository.GetModelListByAsync(
                m => m.Specificulture == culture && m.Status == MixContentStatus.Published,
                "CreatedDateTime",
                DisplayDirection.Desc, pageSize, pageIndex);
        }
        public static async Task<dynamic> GetPostsByAuthor(string username, string culture, int? pageSize = 15, int? pageIndex = 0)
        {
            return await MixPosts.ReadMvcViewModel.Repository.GetModelListByAsync(
                m => m.Specificulture == culture && m.Status == MixContentStatus.Published
                && m.CreatedBy == username,
                "CreatedDateTime",
                DisplayDirection.Desc, pageSize, pageIndex);
        }
        public static async Task<dynamic> GetTagInfo(string keyword, string culture)
        {
            var result = (await MixDatas.Helper.GetSingleDataAsync<MixDatas.ReadMvcViewModel>(keyword, "slug", "sys_tag", culture))?.Data?.Obj;
            return  result as dynamic;
        }

        public static async Task<dynamic> GetAuthorInfo(string keyword, string culture)
        {
            return (await MixDatas.Helper.GetSingleDataByParentIdAsync<MixDatas.ReadMvcViewModel>(keyword, MixDatabaseParentType.User, culture))?.Data?.Obj as dynamic;
        }

        public static async Task<dynamic> GetNextPrevPostByMeta(MixPosts.ReadMvcViewModel currentPost, string culture)
        {
            using (var ctx = new MixCmsContext())
            {
                Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
                var metaId = currentPost.SysTags.FirstOrDefault()?.AttributeData.Id;
                metaId ??= currentPost.SysCategories.FirstOrDefault()?.AttributeData.Id;
                if (metaId != null)
                {

                    var postIdsByMeta = await ctx.MixDatabaseDataAssociation.Where(
                            p => p.DataId == metaId && p.ParentType == MixDatabaseParentType.Post && p.Specificulture == culture)
                        .Select(p => int.Parse(p.ParentId))
                        .ToListAsync();
                    var prevId = postIdsByMeta.OrderBy(p => p).Where(p => p < currentPost.Id).FirstOrDefault();
                    var nextId = postIdsByMeta.OrderBy(p => p).Where(p => p > currentPost.Id).FirstOrDefault();
                    result["PreviousPost"] = prevId > 0 ? new MixPosts.ReadMvcViewModel(ctx.MixPost.Single(m => m.Id == prevId && m.Specificulture == culture)) : null;
                    result["NextPost"] = nextId > 0 ? new MixPosts.ReadMvcViewModel(ctx.MixPost.Single(m => m.Id == nextId && m.Specificulture == culture)) : null;                     
                }
                else
                {
                    var prevPost = ctx.MixPost.Where(
                            m => m.Specificulture == culture && m.Id < currentPost.Id)
                        .OrderBy(m => m.Id).FirstOrDefault();
                    var nextPost = ctx.MixPost.Where(
                            m => m.Specificulture == culture && m.Id > currentPost.Id)
                        .OrderBy(m => m.Id).FirstOrDefault();
                    result["PreviousPost"] = prevPost != null ? new MixPosts.ReadMvcViewModel(prevPost) : null;
                    result["NextPost"] = nextPost != null ? new MixPosts.ReadMvcViewModel(nextPost) : null;
                }
                return result;
            }
        }

        public static string ImgUrl(string Url, string size)
        {
            string[] tmp = Url.Split('.');
            string rtn = "";
            for (int i = 0; i < tmp.Length; i++)
            {
                rtn += tmp[i] + (i < tmp.Length-2?".":"");
                if (i == tmp.Length - 2)
                {
                    rtn += "_" + size + ".";
                }
            }
            return rtn;
        }

        public static int CountWord(string Content)
        {
            //Convert the string into an array of words
            string[] source = Content.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query.  Use ToLowerInvariant to match "data" and "Data"
            //var matchQuery = from word in source
            //                where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
            //                select word;

            // Count the matches, which executes the query.
            return source.Length;
        }

        public static string CountReadingTime(string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return string.Empty;
            }

            int totalSeconds = CountWord(Content) * 1 / 4;
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;
            string time = minutes + ":" + seconds;

            return time;
        }
    }
}