using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Entities.Mix;

namespace Mix.Services.Ecommerce.Lib.ViewModels
{
    [GenerateRestApiController]
    public class OrderItemViewModel : ViewModelBase<EcommerceDbContext, OrderItem, int, OrderItemViewModel>
    {
        #region Properties
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ReferenceUrl { get; set; }
        public string? Currency { get; set; }
        public int PostId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public int OrderId { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Contructors

        public OrderItemViewModel()
        {
        }

        public OrderItemViewModel(EcommerceDbContext context) : base(context)
        {
        }

        public OrderItemViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public OrderItemViewModel(OrderItem entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task<OrderItem> ParseEntity(CancellationToken cancellationToken = default)
        {
            Calculate();
            return base.ParseEntity(cancellationToken);
        }

        #endregion

        #region Methods

        public void Calculate()
        {
            Price = Context.ProductDetails.SingleOrDefault(m => m.ParentId == PostId)?.Price ?? 0;
            Total = Price * Quantity;
        }
        #endregion
    }
}
