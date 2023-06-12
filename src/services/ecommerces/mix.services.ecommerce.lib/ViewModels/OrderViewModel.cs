using Microsoft.Build.Framework;
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
        public Guid TempId { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Currency { get; set; } = "USD";
        public PaymentGateway? PaymentGateway { get; set; }
        public double? Total { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        [Required]
        public string? Email { get; set; }

        public Shipping Shipping { get; set; } = new();
        public JObject PaymentRequest { get; set; }
        public JObject PaymentResponse { get; set; }

        public OrderAddress Address { get; set; }
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
            Address = !string.IsNullOrEmpty(Shipping.ShippingAddress) ? JObject.Parse(Shipping.ShippingAddress).ToObject<OrderAddress>() : new();
        }

        public override async Task<OrderDetail> ParseEntity(CancellationToken cancellationToken = default)
        {
            var result = await base.ParseEntity(cancellationToken);
            if (Address != null)
            {
                result.ShippingAddress = ReflectionHelper.ParseObject(Address)?.ToString(Formatting.None);
            }
            return result;
        }

        protected override async Task SaveEntityRelationshipAsync(OrderDetail parentEntity, CancellationToken cancellationToken = default)
        {
            foreach (var item in OrderItems)
            {
                item.SetUowInfo(UowInfo, CacheService);
                item.OrderDetailId = parentEntity.Id;
                item.MixTenantId = MixTenantId;
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

        public void Calculate(double exchangeRate)
        {
            Total = 0;
            if (!OrderItems.Any(m => m.Title == "Shipping"))
            {
                OrderItems.Add(new(UowInfo)
                {
                    Title = "Shipping",
                    Price = Shipping.ShippingFee,
                    Currency = Currency,
                    Quantity = 1,
                    MixTenantId = MixTenantId,
                    CreatedBy = CreatedBy
                });
            }
            foreach (var item in OrderItems)
            {
                if (item.Currency != Currency)
                {
                    item.Currency = Currency;
                    item.Price = Math.Round(item.Price / exchangeRate, 2);
                    Total += item.Price;
                }
                else
                {
                    Total += item.Price;
                }
            }
        }

        #endregion
    }
    public class Shipping
    {
        public double ShippingFee { get; set; }
        public string Currency { get; set; }
        public string ShippingAddress { get; set; }
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
        public string CountryCode { get; set; } = "US";
        public string PostalCode { get; set; } = "US";

        public override string ToString()
        {
            return $"{Street} {Ward} {District} {City} {Province} {PostalCode} {CountryCode}";
        }
    }
}
