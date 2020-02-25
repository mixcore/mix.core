using System;

namespace Mix.Cms.Lib.Models.Cms
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