using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixMedia
    {
        public MixMedia()
        {
            MixArticleMedia = new HashSet<MixArticleMedia>();
            MixProductMedia = new HashSet<MixProductMedia>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public string FileProperties { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }

        public ICollection<MixArticleMedia> MixArticleMedia { get; set; }
        public ICollection<MixProductMedia> MixProductMedia { get; set; }
    }
}
