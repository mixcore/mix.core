namespace Mix.Services.Ecommerce.Lib.Dtos
{
    public class CartItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ReferenceUrl { get; set; }
        public int PostId { get; set; }
        public int Quantity { get; set; }
        //public string? Currency { get; set; }
        //public double Price { get; set; }
    }
}
