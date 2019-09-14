using System;
using System.Collections.Generic;

namespace Mix.Cms.Web.Models.Cms
{
    public partial class MixCache
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public DateTime? ExpiredDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Status { get; set; }
    }
}
