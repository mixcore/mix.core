using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixFile
    {
        public int Id { get; set; }
        public string StringContent { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public string FolderType { get; set; }
        public int? ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }

        public virtual MixTheme Theme { get; set; }
    }
}
