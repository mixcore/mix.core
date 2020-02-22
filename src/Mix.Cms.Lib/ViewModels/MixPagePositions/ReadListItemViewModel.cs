using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPagePositions
{
    public class ReadListItemViewModel
       : ViewModelBase<MixCmsContext, MixPagePosition, ReadListItemViewModel>
    {
        public ReadListItemViewModel(MixPagePosition model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadListItemViewModel() : base()
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

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}