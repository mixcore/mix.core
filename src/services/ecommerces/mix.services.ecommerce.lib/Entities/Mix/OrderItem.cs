using Mix.Heart.Entities;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix
{
    public class OrderItem : EntityBase<int>
    {
        public string? Sku { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? ReferenceUrl { get; set; }
        public string? Currency { get; set; }
        public int PostId { get; set; }
        public double? Price { get; set; }
        public double? Percent { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public int OrderId { get; set; }
        public int MixTenantId { get; set; }
    }
}
