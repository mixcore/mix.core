using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixCulture
    {
        public MixCulture()
        {
            MixArticle = new HashSet<MixArticle>();
            MixCategory = new HashSet<MixCategory>();
            MixConfiguration = new HashSet<MixConfiguration>();
            MixLanguage = new HashSet<MixLanguage>();
            MixModule = new HashSet<MixModule>();
            MixProduct = new HashSet<MixProduct>();
            MixUrlAlias = new HashSet<MixUrlAlias>();
        }

        public int Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public string Lcid { get; set; }
        public int Priority { get; set; }
        public string Specificulture { get; set; }
        public int Status { get; set; }

        public ICollection<MixArticle> MixArticle { get; set; }
        public ICollection<MixCategory> MixCategory { get; set; }
        public ICollection<MixConfiguration> MixConfiguration { get; set; }
        public ICollection<MixLanguage> MixLanguage { get; set; }
        public ICollection<MixModule> MixModule { get; set; }
        public ICollection<MixProduct> MixProduct { get; set; }
        public ICollection<MixUrlAlias> MixUrlAlias { get; set; }
    }
}
