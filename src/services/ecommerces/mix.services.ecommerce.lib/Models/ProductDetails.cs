namespace Mix.Services.Ecommerce.Lib.Models
{
    public class ProductDetails
    {
        public int Id { get; set; }
        public double? Price { get; set; }
        public ProductMetadata Metadata { get; set; }
    }

    public class ProductMetadata
    {
        public string[] Tile { get; set; }
        public string[] Interior { get; set; }
        public string[] Lighting { get; set; }
        public string[] Decor { get; set; }
        public string[] Brands { get; set; }
    }
}
