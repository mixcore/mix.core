using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixMedias;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPostMedias
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPostMedia, ReadViewModel>
    {
        public ReadViewModel(MixPostMedia model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }

        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("mediaId")]
        public int MediaId { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

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

        public UpdateViewModel Media { get; set; }

        #endregion Views

        #endregion Properties

        #region overrides

        public override MixPostMedia ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id, _context, _transaction).Data + 1;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getMedia = UpdateViewModel.Repository.GetSingleModel(p => p.Id == MediaId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getMedia.IsSucceed)
            {
                Media = getMedia.Data;
            }
        }

        public override RepositoryResponse<bool> SaveSubModels(MixPostMedia parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var saveMedia = Media.SaveModel(false, _context, _transaction);
            if (!saveMedia.IsSucceed)
            {
                result.IsSucceed = false;
                result.Exception = saveMedia.Exception;
                result.Errors = saveMedia.Errors;
            }
            return result;
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPostMedia parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var saveMedia = await Media.SaveModelAsync(false, _context, _transaction);
            if (!saveMedia.IsSucceed)
            {
                result.IsSucceed = false;
                result.Exception = saveMedia.Exception;
                result.Errors = saveMedia.Errors;
            }
            return result;
        }

        #endregion Async

        #endregion overrides
    }
}