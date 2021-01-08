﻿using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixTheme
    {
        public MixTheme()
        {
            MixFile = new HashSet<MixFile>();
            MixTemplate = new HashSet<MixTemplate>();
        }

        public int Id { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string PreviewUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual ICollection<MixFile> MixFile { get; set; }
        public virtual ICollection<MixTemplate> MixTemplate { get; set; }
    }
}
