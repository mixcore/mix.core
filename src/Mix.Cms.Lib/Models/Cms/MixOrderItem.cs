using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixOrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string PriceUnit { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public MixOrder MixOrder { get; set; }
        public MixProduct MixProduct { get; set; }
    }
}
