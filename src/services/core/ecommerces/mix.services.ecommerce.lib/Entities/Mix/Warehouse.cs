using Mix.Heart.Entities;

namespace Mix.Services.Ecommerce.Lib.Entities
{
    public class Warehouse : EntityBase<int>
    {
        public int TenantId { get; set; }
        public int PostId { get; set; }
        public string? Sku { get; set; }
        public string Currency { get; set; }
        public double? Price { get; set; }
        public int? InStock { get; set; }
        public int? Sold { get; set; }
    }
}
