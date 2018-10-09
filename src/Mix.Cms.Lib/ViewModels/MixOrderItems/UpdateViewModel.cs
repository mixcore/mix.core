using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixOrderItems
{
    public class UpdateViewModel
        : ViewModelBase<MixCmsContext, MixOrderItem, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        [JsonProperty("productId")]
        public int ProductId { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("price")]
        public double Price { get; set; }
        [JsonProperty("priceUnit")]
        public string PriceUnit { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonIgnore]
        [JsonProperty("storeId")]
        public int StoreId { get; set; }
        [JsonProperty("status")]
        public MixEnums.MixOrderStatus Status { get; set; }
        #endregion

        #region Views

        public MixProducts.ReadListItemViewModel Product { get; set; }

        #endregion

        #endregion

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixOrderItem model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void Validate(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (!Repository.CheckIsExists(o => o.Id == OrderId, _context, _transaction))
                {
                    Errors.Add("Invalid Order");
                    IsValid = false;
                }
                if (!MixProducts.ReadListItemViewModel.Repository.CheckIsExists(p => p.Id == ProductId && p.Specificulture == Specificulture, _context, _transaction))
                {
                    Errors.Add("Invalid Product");
                    IsValid = false;
                }
            }
        }

        public override MixOrderItem ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var product = MixProducts.ReadListItemViewModel.Repository.GetSingleModel(p => p.Id == ProductId && p.Specificulture == Specificulture).Data;
            Price = product?.Price ?? 0;
            Quantity = 1;
            PriceUnit = product?.PriceUnit;
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Product = MixProducts.ReadListItemViewModel.Repository.GetSingleModel(p => p.Id == ProductId && p.Specificulture == Specificulture, _context, _transaction).Data;
        }
        #endregion Overrides
    }
}
