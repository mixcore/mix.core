using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixSetAttribute
    {
        public MixSetAttribute()
        {
            MixArticle = new HashSet<MixArticle>();
            MixPage = new HashSet<MixPage>();
            MixProduct = new HashSet<MixProduct>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Fields { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }

        public ICollection<MixArticle> MixArticle { get; set; }
        public ICollection<MixPage> MixPage { get; set; }
        public ICollection<MixProduct> MixProduct { get; set; }
    }
}
