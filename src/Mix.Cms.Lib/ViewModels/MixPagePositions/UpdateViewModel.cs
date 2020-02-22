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

        [JsonProperty("pageId")]
        public int PageId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonRequired]
        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        [JsonProperty("page")]
        public MixPages.ReadListItemViewModel Page { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Page = MixPages.ReadListItemViewModel.Repository.GetSingleModel(p => p.Id == PageId && p.Specificulture == Specificulture, _context, _transaction).Data;
        }

        #endregion overrides
    }
}