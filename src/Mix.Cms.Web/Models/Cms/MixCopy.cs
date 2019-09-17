using System;
using System.Collections.Generic;

namespace Mix.Cms.Web.Models.Cms
{
    public partial class MixCopy
    {
        public string Culture { get; set; }
        public string Keyword { get; set; }
        public string Note { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Value { get; set; }
    }
}
