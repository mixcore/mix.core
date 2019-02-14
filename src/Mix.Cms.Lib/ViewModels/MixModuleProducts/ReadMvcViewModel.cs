using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixModuleProducts
{
    public class ReadMvcViewModel
       : ViewModelBase<MixCmsContext, MixModuleProduct, ReadMvcViewModel>
    {
        public ReadMvcViewModel(MixModuleProduct model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadMvcViewModel() : base()
        {
        }

        [JsonProperty("ProductId")]
        public int ProductId { get; set; }

        [JsonProperty("ModuleId")]
        public int ModuleId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        public MixProducts.ReadMvcViewModel Product { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getProduct = MixProducts.ReadMvcViewModel.Repository.GetSingleModel(p => p.Id == ProductId && p.Specificulture == Specificulture
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
