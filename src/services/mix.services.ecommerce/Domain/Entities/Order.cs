using mix.services.ecommerce.Domain.Enums;
using Mix.Heart.Entities;

namespace mix.services.ecommerce.Domain.Entities
{
    public class Order : EntityBase<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public double? Total { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int MixTenantId { get; set; }
    }
}
