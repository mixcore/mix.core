using Mix.Heart.Entities;
using Mix.Services.Ecommerce.Lib.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix
{
    public class OrderDetail : EntityBase<int>
    {
        public Guid TempId { get; set; }
        public string? Title { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
        public double? Total { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string Email { get; set; }
        public JObject? ShippingAddress { get; set; }
        public JObject? PaymentRequest { get; set; }
        public JObject? PaymentResponse { get; set; }
        public int MixTenantId { get; set; }
    }
}
