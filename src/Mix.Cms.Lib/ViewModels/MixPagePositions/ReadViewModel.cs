using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPagePositions
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPagePosition, ReadViewModel>
    {
        public ReadViewModel(MixPagePosition model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
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

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        public MixPositions.ReadViewModel Position { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPosition = MixPositions.ReadViewModel.Repository.GetSingleModel(p => p.Id == PositionId
                , _context: _context, _transaction: _transaction
            );
            if (getPosition.IsSucceed)
            {
                Position = getPosition.Data;
            }
        }

        #region Async

        #endregion Async

        #endregion overrides
    }
}
