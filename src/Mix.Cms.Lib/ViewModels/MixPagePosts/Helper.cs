using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixPagePosts
{
    public class Helper
    {
        public static RepositoryResponse<List<MixPagePosts.ReadViewModel>> GetNavAsync(int postId, string specificulture
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var navCategoryPostViewModels = context.MixPage.Include(cp => cp.MixPagePost).Where(a => a.Specificulture == specificulture
                    && (a.Type == MixPageType.ListPost)
                    )
                    .AsEnumerable()
                    .Select(p => new MixPagePosts.ReadViewModel(
                        new MixPagePost()
                        {
                            PostId = postId,
                            PageId = p.Id,
                            Specificulture = specificulture
                        },
                        _context, _transaction)
                    {
                        IsActived = p.MixPagePost.Any(cp => cp.PostId == postId && cp.Specificulture == specificulture),
                        Description = p.Title
                    });
                return new RepositoryResponse<List<ReadViewModel>>()
                {
                    IsSucceed = true,
                    Data = navCategoryPostViewModels.ToList()
                };
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                if (isRoot)
                {
                    transaction.Rollback();
                }
                return new RepositoryResponse<List<MixPagePosts.ReadViewModel>>()
                {
                    IsSucceed = true,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static RepositoryResponse<List<TView>> GetActivedNavAsync<TView>(
            int? postId
            , int? pageId = null
            , string specificulture = null
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPagePost, TView>
        {
            if (postId.HasValue || pageId.HasValue)
            {
                specificulture = specificulture ?? MixService.GetAppSetting<string>("DefaultCulture");
                var result = DefaultRepository<MixCmsContext, MixPagePost, TView>.Instance.GetModelListBy(
                    m => (!postId.HasValue || m.PostId == postId.Value)
                        && (!pageId.HasValue || m.PageId == pageId.Value)
                        && m.Specificulture == specificulture, _context, _transaction);
                return result;
            }
            else
            {
                return new RepositoryResponse<List<TView>>();
            }
        }
    }
}