﻿using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixMedia
    {
        public MixMedia()
        {
            MixPostMedia = new HashSet<MixPostMedia>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public string FileProperties { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Source { get; set; }
        public string TargetUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual ICollection<MixPostMedia> MixPostMedia { get; set; }
    }
}
