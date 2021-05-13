using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixPagePosts
{
    public class ImportViewModel
       : ViewModelBase<MixCmsContext, MixPagePost, ImportViewModel>
    {
        public ImportViewModel(MixPagePost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ImportViewModel() : base()
        {
        }

        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

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

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("post")]
        public MixPosts.ImportViewModel Post { get; set; }

        [JsonProperty("page")]
        public MixPages.ImportViewModel Page { get; set; }

        #endregion Views

        #endregion Properties

        #region overrides

        public override MixPagePost ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id, _context, _transaction).Data + 1;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPost = MixPosts.ImportViewModel.Repository.GetSingleModel(p => p.Id == PostId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getPost.IsSucceed)
            {
                Post = getPost.Data;
            }
        }

        #endregion overrides

        #region Expand

        public static RepositoryResponse<List<MixPagePosts.ImportViewModel>> GetPagePostNavAsync(int postId, string specificulture
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var navCategoryPostViewModels = context.MixPage.Include(cp => cp.MixPagePost).Where(a => a.Specificulture == specificulture
                    && (a.Type == MixPageType.ListPost)
                    )
                    .AsEnumerable()
                    .Select(p => new MixPagePosts.ImportViewModel(
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
                return new RepositoryResponse<List<ImportViewModel>>()
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
                return new RepositoryResponse<List<MixPagePosts.ImportViewModel>>()
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

        #endregion Expand
    }
}