using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class DeleteViewModel
         : ViewModelBase<MixCmsContext, MixPost, DeleteViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        public string Type { get; set; }

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

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #region Async Methods

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>()
            {
                IsSucceed = true
            };

            if (result.IsSucceed)
            {
                var navCate = await _context.MixPagePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navCate)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navModule = await _context.MixModulePost.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navMedia = await _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navMedia)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            if (result.IsSucceed)
            {
                var navModule = await _context.MixPostModule.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navModule)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navRelated = await _context.MixPostMedia.Where(n => n.PostId == Id && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navRelated)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = await _context.MixUrlAlias.Where(n => n.SourceId == Id.ToString() && n.Type == (int)MixUrlAliasType.Post && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = await _context.MixRelatedPost.Where(n => (n.Id == Id || n.DestinationId == Id) && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            if (result.IsSucceed)
            {
                var navs = await _context.MixDatabaseDataAssociation.Where(n => n.ParentId == Id.ToString() && n.ParentType == MixDatabaseParentType.Post && n.Specificulture == Specificulture).ToListAsync();
                foreach (var item in navs)
                {
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }

            await _context.SaveChangesAsync();
            var removeRelatedData = await MixDatabaseDataAssociations.Helper.RemoveRelatedDataAsync(
                    Id.ToString(), MixDatabaseParentType.Post
                    , Specificulture
                    , _context, _transaction);
            ViewModelHelper.HandleResult(removeRelatedData, ref result);
            return result;
        }

        #endregion Async Methods

        #endregion Overrides
    }
}