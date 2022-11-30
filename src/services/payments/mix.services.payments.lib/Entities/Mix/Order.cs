using Mix.Heart.Entities;
using Mix.Services.Payments.Lib.Enums;

namespace Mix.Services.Payments.Lib.Entities.Mix
{
    public class Order : EntityBase<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
        public double? Total { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int MixTenantId { get; set; }
    }
}
