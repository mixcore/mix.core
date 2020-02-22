namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixOrderItem
    {
        public int OrderId { get; set; }
        public int PostId { get; set; }
        public string Specificulture { get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string PriceUnit { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public virtual MixOrder MixOrder { get; set; }
        public virtual MixPost MixPost { get; set; }
    }
}