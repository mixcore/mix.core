using Mix.Constant.Enums;
using Mix.Heart.Entities;

namespace Mix.Services.Ecommerce.Lib.Entities
{
    public class ProductDetails: EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public string? Thumbnail { get; set; }
    }
}
