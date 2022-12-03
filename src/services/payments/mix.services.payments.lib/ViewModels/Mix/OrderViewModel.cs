using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Payments.Lib.Entities.Mix;
using Mix.Services.Payments.Lib.Enums;

namespace Mix.Services.Payments.Lib.ViewModels.Mix
{
    public class OrderViewModel : ViewModelBase<PaymentDbContext, Order, int, OrderViewModel>
    {
        #region Properties

        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public double Total { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int MixTenantId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new();

        #endregion

        #region Contructors

        public OrderViewModel()
        {
        }

        public OrderViewModel(PaymentDbContext context) : base(context)
        {
        }

        public OrderViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public OrderViewModel(Order entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            OrderItems = await OrderItemViewModel.GetRepository(UowInfo).GetListAsync(m => m.MixTenantId == MixTenantId && m.OrderId == Id);
        }

        protected override async Task SaveEntityRelationshipAsync(Order parentEntity, CancellationToken cancellationToken = default)
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
    }
}
