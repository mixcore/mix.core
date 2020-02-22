using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixPagePosts
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPagePost, ReadViewModel>
    {
        public ReadViewModel(MixPagePost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("pageId")]
        public int PageId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #region Views

        [JsonProperty("post")]
        public MixPosts.ReadListItemViewModel Post { get; set; }

        [JsonProperty("page")]
        public MixPages.ReadViewModel Page { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPost = MixPosts.ReadListItemViewModel.Repository.GetSingleModel(p => p.Id == PostId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            var getPage = MixPages.ReadViewModel.Repository.GetSingleModel(p => p.Id == PageId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getPost.IsSucceed)
            {
                Post = getPost.Data;
            }
            if (getPage.IsSucceed)
            {
                Page = getPage.Data;
            }
        }

        #endregion overrides

        #region Expand

        public static RepositoryResponse<List<MixPagePosts.ReadViewModel>> GetPagePostNavAsync(int postId, string specificulture
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            MixCmsContext context = _context ?? new MixCmsContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var navCategoryPostViewModels = context.MixPage.Include(cp => cp.MixPagePost).Where(a => a.Specificulture == specificulture
                    && (a.Type == (int)MixEnums.MixPageType.ListPost)
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
                    context.Dispose();
                }
            }
        }

        #endregion Expand
    }
}