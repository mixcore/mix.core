using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPagePositions
{
    public class DeleteViewModel
       : ViewModelBase<MixCmsContext, MixPagePosition, DeleteViewModel>
    {
        public DeleteViewModel(MixPagePosition model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public DeleteViewModel() : base()
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
    }
}