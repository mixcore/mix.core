using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public PaymentStatus PaymentStatus { get; set; }
        public string Email { get; set; }

        public string? ShippingAddress { get; set; }
        public string? PaymentRequest { get; set; }
        public string? PaymentResponse { get; set; }

        public OrderAddress Address { get; set; }
        public JObject PaymentRequestData { get; set; }
        public JObject PaymentResponseData { get; set; }
        public int MixTenantId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new();
        public List<OrderTrackingViewModel> OrderTrackings { get; set; } = new();

        #endregion

        #region Contructors

        public OrderViewModel()
        {
            IsCache = false;
        }

        public OrderViewModel(EcommerceDbContext context) : base(context)
        {
            IsCache = false;
        }

        public OrderViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public OrderViewModel(OrderDetail entity, UnitOfWorkInfo? uowInfo = null) : base(entity, uowInfo)
        {
            IsCache = false;
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            OrderItems = await OrderItemViewModel.GetRepository(UowInfo, CacheService).GetListAsync(m => m.MixTenantId == MixTenantId && m.OrderDetailId == Id, cancellationToken);
            OrderTrackings = await OrderTrackingViewModel.GetRepository(UowInfo, CacheService).GetListAsync(m => m.MixTenantId == MixTenantId && m.OrderDetailId == Id, cancellationToken);
            Address = !string.IsNullOrEmpty(ShippingAddress) ? JObject.Parse(ShippingAddress).ToObject<OrderAddress>() : new();
            PaymentRequestData = !string.IsNullOrEmpty(PaymentRequest) ? JObject.Parse(PaymentRequest) : new();
            PaymentResponseData = !string.IsNullOrEmpty(PaymentResponse) ? JObject.Parse(PaymentResponse) : new();
        }

        public override async Task<OrderDetail> ParseEntity(CancellationToken cancellationToken = default)
        {
            Calculate();
            var result = await base.ParseEntity(cancellationToken);
            if (Address != null)
            {
                result.ShippingAddress = ReflectionHelper.ParseObject(Address)?.ToString(Formatting.None);
            }
            result.PaymentRequest = PaymentRequestData?.ToString(Formatting.None);
            result.PaymentResponse = PaymentResponseData?.ToString(Formatting.None);
            return result;
        }

        protected override async Task SaveEntityRelationshipAsync(OrderDetail parentEntity, CancellationToken cancellationToken = default)
        {
            foreach (var item in OrderItems)
            {
                item.SetUowInfo(UowInfo, CacheService);
                item.OrderDetailId = parentEntity.Id;
                await item.SaveAsync(cancellationToken);
            }
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in OrderItems)
            {
                item.SetUowInfo(UowInfo, CacheService);
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

    public class OrderAddress
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Ward { get; set; }
    }
}
