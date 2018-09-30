using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPageProducts
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPageProduct, ReadViewModel>
    {
        public ReadViewModel(MixPageProduct model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }

        [JsonProperty("ProductId")]
        public int ProductId { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        public MixProducts.ReadListItemViewModel Product { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getProduct = MixProducts.ReadListItemViewModel.Repository.GetSingleModel(p => p.Id == ProductId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getProduct.IsSucceed)
            {
                Product = getProduct.Data;
            }
        }

        #region Async

        #endregion Async

        #endregion overrides
    }
}
