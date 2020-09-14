using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
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
            MixCmsContext context = _context ?? new MixCmsContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var navCategoryPostViewModels = context.MixPage.Include(cp => cp.MixPagePost).Where(a => a.Specificulture == specificulture
                    && (a.Type == MixEnums.MixPageType.ListPost.ToString())
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
                if (_transaction == null)
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
                if (_context == null)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    context.Database.CloseConnection(); transaction.Dispose(); context.Dispose();
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
                specificulture = specificulture ?? MixService.GetConfig<string>("DefaultCulture");
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