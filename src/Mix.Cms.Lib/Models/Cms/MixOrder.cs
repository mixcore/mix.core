using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixOrder
    {
        public MixOrder()
        {
            MixComment = new HashSet<MixComment>();
            MixOrderItem = new HashSet<MixOrderItem>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public int StoreId { get; set; }
        public string Specificulture { get; set; }
        public int Status { get; set; }

        public MixCustomer Customer { get; set; }
        public ICollection<MixComment> MixComment { get; set; }
        public ICollection<MixOrderItem> MixOrderItem { get; set; }
    }
}
