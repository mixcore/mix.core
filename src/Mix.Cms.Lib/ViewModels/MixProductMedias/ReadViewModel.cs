using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixProductMedias
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixProductMedia, ReadViewModel>
    {
        public ReadViewModel(MixProductMedia model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }

        [JsonProperty("mediaId")]
        public int MediaId { get; set; }

        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        public MixMedias.UpdateViewModel Media { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getMedia = MixMedias.UpdateViewModel.Repository.GetSingleModel(p => p.Id == MediaId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getMedia.IsSucceed)
            {
                Media = getMedia.Data;
            }
        }

        #region Async

        #endregion Async

        #endregion overrides
    }
}
