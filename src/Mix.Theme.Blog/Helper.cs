using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MixDatas = Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using MixPosts = Mix.Cms.Lib.ViewModels.MixPosts;

namespace Mix.Theme.Blog
{
    public static class Helper
    {
        public static async Task<dynamic> GetPosts(string culture = null, int? pageSize = 15, int? pageIndex = 0)
        {
            return await MixPosts.ReadMvcViewModel.Repository.GetModelListByAsync(
                m => m.Status == MixContentStatus.Published,
                "CreatedDateTime",
                MixHeartEnums.DisplayDirection.Desc, pageSize, pageIndex);
        }
        public static async Task<dynamic> GetPostsByAuthor(string username, string culture, int? pageSize = 15, int? pageIndex = 0)
        {
            return await MixPosts.ReadMvcViewModel.Repository.GetModelListByAsync(
                m => m.Status == MixContentStatus.Published
                && m.CreatedBy == username,
                "CreatedDateTime",
                MixHeartEnums.DisplayDirection.Desc, pageSize, pageIndex);
        }
        public static async Task<dynamic> GetTagInfo(string keyword, string culture)
        {
            return (await MixDatas.Helper.GetSingleDataAsync<MixDatas.ReadMvcViewModel>(keyword, "slug", "sys_tag", culture)).Data.Obj as dynamic;
        }

        public static async Task<dynamic> GetAuthorInfo(string keyword, string culture)
        {
            return (await MixDatas.Helper.GetSingleDataByParentIdAsync<MixDatas.ReadMvcViewModel>(keyword, MixDatabaseParentType.User, culture))?.Data?.Obj as dynamic;
        }

        public static async Task<dynamic> GetNextPrevPostByTag(MixPosts.ReadMvcViewModel currentPost, string culture)
        {
            using (var ctx = new MixCmsContext())
            {
                var tagId = currentPost.SysTags.FirstOrDefault()?.AttributeData.Id;
                if (tagId != null)
                {

                    var postIdsByTag = await ctx.MixDatabaseDataAssociation.Where(
                            p => p.DataId == tagId && p.ParentType == MixDatabaseParentType.Post && p.Specificulture == culture)
                        .Select(p => int.Parse(p.ParentId))
                        .ToListAsync();
                    var prevId = postIdsByTag.OrderBy(p => p).Where(p => p < currentPost.Id).FirstOrDefault();
                    var nextId = postIdsByTag.OrderBy(p => p).Where(p => p > currentPost.Id).FirstOrDefault();
                    return new
                    {
                        PreviousPost = prevId > 0 ? new MixPosts.ReadMvcViewModel(ctx.MixPost.Single(m => m.Id == prevId)) : null,
                        NextPost = nextId > 0 ? new MixPosts.ReadMvcViewModel(ctx.MixPost.Single(m => m.Id == nextId)) : null
                    };
                }
                else
                {
                    var prevPost = ctx.MixPost.Where(
                            m => m.Specificulture == culture && m.Id < currentPost.Id)
                        .OrderBy(m => m.Id).FirstOrDefault();
                    var nextPost = ctx.MixPost.Where(
                            m => m.Specificulture == culture && m.Id > currentPost.Id)
                        .OrderBy(m => m.Id).FirstOrDefault();
                    return new
                    {
                        PreviousPost = prevPost != null ? new MixPosts.ReadMvcViewModel(prevPost) : null,
                        NextPost = nextPost != null ? new MixPosts.ReadMvcViewModel(nextPost) : null
                    };
                }
            }
        }

        public static string ImgUrl(string Url, string size)
        {
            string[] tmp = Url.Split('.');
            string rtn = "";
            for (int i = 0; i < tmp.Length; i++)
            {
                rtn += tmp[i];
                if (i < tmp.Length - 1)
                {
                    rtn += "_" + size + ".";
                }
            }
            return Url;
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
            int totalSeconds = CountWord(Content) * 1 / 4;
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;
            string time = minutes + ":" + seconds;

            return time;
        }
    }
}