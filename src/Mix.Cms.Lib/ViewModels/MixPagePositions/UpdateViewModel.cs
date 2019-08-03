using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPagePositions
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixPagePosition, UpdateViewModel>
    {
        public UpdateViewModel(MixPagePosition model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
        {
        }

        [JsonProperty("PositionId")]
        public int PositionId { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonRequired]
        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        #endregion Views

        #region overrides

        #region Async

        #endregion Async

        #endregion overrides
    }
}
