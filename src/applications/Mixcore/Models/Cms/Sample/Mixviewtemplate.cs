using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixviewtemplate
{
    public int Id { get; set; }

    public int MixTenantId { get; set; }

    public string Content { get; set; }

    public string Extension { get; set; }

    public string FileFolder { get; set; }

    public string FileName { get; set; }

    public string FolderType { get; set; }

    public string Scripts { get; set; }

    public string Styles { get; set; }

    public string MixThemeName { get; set; }

    public int MixThemeId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Mixtheme MixTheme { get; set; }
}
