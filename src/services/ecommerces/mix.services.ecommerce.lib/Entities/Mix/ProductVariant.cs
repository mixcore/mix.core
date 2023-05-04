using Mix.Heart.Entities;

namespace Mix.Services.Ecommerce.Lib.Entities
{
    public class ProductVariant : EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public int ProductDetailsId { get; set; }
        public string? Sku { get; set; }
        public double? Price { get; set; }
        public int? Inventory { get; set; }
        public int? Sold { get; set; }
    }
}
