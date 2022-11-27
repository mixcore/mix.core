using mix.services.ecommerce.Domain.Enums;
using Mix.Heart.Entities;

namespace mix.services.ecommerce.Domain.Entities
{
    public class OrderItem : EntityBase<int>
    {
        public int MixTenantId { get; set; }
    }
}
