namespace Mix.Services.Ecommerce.Lib.Models
{
    public class ProductDetailsModel
    {
        public int Id { get; set; }
        public double? Price { get; set; }
        public string DesignBy { get; set; }
        public string Information { get; set; }
        public string InformationImage { get; set; }
        public string Size { get; set; }
        public string SizeImage{ get; set; }
        public string Document { get; set; }
        public string MaintenanceDocument { get; set; }

    }
}
