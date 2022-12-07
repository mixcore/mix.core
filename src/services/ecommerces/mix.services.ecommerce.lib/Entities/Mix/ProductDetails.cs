using Mix.Constant.Enums;
using Mix.Heart.Entities;

namespace Mix.Services.Ecommerce.Lib.Entities
{
    public class ProductDetails: EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public double? Price { get; set; }
        public ProductMetadata Metadata { get; set; }
    }

    public class ProductMetadata
    {
        public string[]? Tile { get; set; }
        public string[]? Interior { get; set; }
        public string[]? Lighting { get; set; }
        public string[]? Decor { get; set; }
        public string[]? Brands { get; set; }
    }
}
