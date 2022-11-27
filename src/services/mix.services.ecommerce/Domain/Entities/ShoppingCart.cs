using Mix.Heart.Entities;

namespace mix.services.ecommerce.Domain.Entities
{
    public class ShoppingCart : EntityBase<int>
    {
        public Guid UserId { get; set; }
        public int SysUserDataId { get; set; }
        public string Title { get; set; }
        public int MixTenantId { get; set; }
    }
}
