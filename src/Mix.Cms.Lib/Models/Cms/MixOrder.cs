using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixOrder
    {
        public MixOrder()
        {
            MixOrderItem = new HashSet<MixOrderItem>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string UserId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public int StoreId { get; set; }
        public int Status { get; set; }

        public virtual MixCustomer Customer { get; set; }
        public virtual ICollection<MixOrderItem> MixOrderItem { get; set; }
    }
}