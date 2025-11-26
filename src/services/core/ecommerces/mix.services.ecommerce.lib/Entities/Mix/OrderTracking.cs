using Mix.Heart.Entities;
using Mix.Services.Ecommerce.Lib.Enums;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix
{
    public class OrderTracking : EntityBase<int>
    {
        public OrderTrackingAction? Action { get; set; }
        public string? Note { get; set; }
        public int OrderDetailId { get; set; }
        public int TenantId { get; set; }
    }
}
