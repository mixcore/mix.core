using mix.services.ecommerce.Domain.Enums;
using Mix.Heart.Entities;

namespace mix.services.ecommerce.Domain.Entities
{
    public class Order : EntityBase<int>
    {
        public string Title { get; set; }
        public double Total { get; set; }
        public int SysUserDataId { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int MixTenantId { get; set; }
    }
}
