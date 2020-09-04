using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public static RepositoryResponse<List<MixPagePosts.ReadViewModel>> GetActivedNavAsync(
            int postId, string specificulture
            , bool isLoadPage = false, bool isLoadPost = false
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = MixPagePosts.ReadViewModel.Repository.GetModelListBy(
                m => m.PostId == postId && m.Specificulture == specificulture, _context, _transaction);
            result.Data.ForEach(p => {
                p.IsActived = true;
                if (isLoadPage)
                {
                    p.LoadPage(_context, _transaction);
                }
                if (isLoadPost)
                {
                    p.LoadPost(_context, _transaction);
                }
                });
            return result;
        }
    }
}