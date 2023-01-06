using Mix.Heart.Entities;
using Mix.Services.Ecommerce.Lib.Enums;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix
{
    public class OrderDetail : EntityBase<int>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
        public double? Total { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string Email { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PaymentRequest { get; set; }
        public string? PaymentResponse { get; set; }
        public int MixTenantId { get; set; }
    }
}
