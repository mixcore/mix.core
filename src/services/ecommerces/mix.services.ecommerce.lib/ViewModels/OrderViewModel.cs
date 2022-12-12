using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Entities.Mix;

namespace Mix.Services.Ecommerce.Lib.ViewModels
{
    public class OrderViewModel : ViewModelBase<EcommerceDbContext, OrderDetail, int, OrderViewModel>
    {
        #region Properties

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
        public double? Total { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int MixTenantId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new();

        #endregion

        #region Contructors

        public OrderViewModel()
        {
        }

        public OrderViewModel(EcommerceDbContext context) : base(context)
        {
        }

        public OrderViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public OrderViewModel(OrderDetail entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            OrderItems = await OrderItemViewModel.GetRepository(UowInfo).GetListAsync(m => m.MixTenantId == MixTenantId && m.OrderId == Id, cancellationToken);
        }

        public override Task<OrderDetail> ParseEntity(CancellationToken cancellationToken = default)
        {
            Calculate();
            return base.ParseEntity(cancellationToken);
        }

        protected override async Task SaveEntityRelationshipAsync(OrderDetail parentEntity, CancellationToken cancellationToken = default)
        {
            foreach (var item in OrderItems)
            {
                item.SetUowInfo(UowInfo);
                item.OrderId = parentEntity.Id;
                await item.SaveAsync(cancellationToken);
            }
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in OrderItems)
            {
                item.SetUowInfo(UowInfo);
                await item.DeleteAsync(cancellationToken);
            }
            await base.DeleteHandlerAsync(cancellationToken);
        }
        #endregion

        #region Methods

        public void Calculate()
        {
            Total = OrderItems.Sum(o => o.Total);
        }

        #endregion
    }
}
