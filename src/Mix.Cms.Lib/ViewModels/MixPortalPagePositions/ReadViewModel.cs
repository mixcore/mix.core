using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPortalPagePositions
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPortalPagePosition, ReadViewModel>
    {
        public ReadViewModel(MixPortalPagePosition model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }

        [JsonProperty("positionId")]
        public int PositionId { get; set; }
        [JsonProperty("portalPageId")]
        public int PortalPageId { get; set; }
        [JsonProperty("isActived")]
        public bool IsActived { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        public MixPositions.ReadViewModel Position { get; set; }

        #endregion Views

        #region overrides
        public override MixPortalPagePosition ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Priority == 0)
            {
                Priority = Repository.Max(n => n.Priority).Data + 1;
            }
            return base.ParseModel(_context, _transaction);
        }
        #region Async

        #endregion Async

        #endregion overrides
    }
}
