namespace Mix.Services.Ecommerce.Lib.Dtos
{
    public class CartItemDto
    {
        public string Sku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ReferenceUrl { get; set; }
        public int PostId { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; } = true;
        //public string? Currency { get; set; }
        //public double Price { get; set; }
    }
}
