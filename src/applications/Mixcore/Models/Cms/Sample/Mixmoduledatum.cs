using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixmoduledatum
{
    public int Id { get; set; }

    public string SimpleDataColumns { get; set; }

    public string Value { get; set; }

    public int? MixModuleContentId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public int MixTenantId { get; set; }

    public string Specificulture { get; set; }

    public string Icon { get; set; }

    public bool IsPublic { get; set; }

    public int ParentId { get; set; }

    public int MixCultureId { get; set; }

    public string Title { get; set; }

    public string Excerpt { get; set; }

    public string Content { get; set; }

    public int? LayoutId { get; set; }

    public int? TemplateId { get; set; }

    public string Image { get; set; }

    public string Source { get; set; }

    public string SeoDescription { get; set; }

    public string SeoKeywords { get; set; }

    public string SeoName { get; set; }

    public string SeoTitle { get; set; }

    public DateTime? PublishedDateTime { get; set; }

    public virtual Mixculture MixCulture { get; set; }

    public virtual Mixmodulecontent MixModuleContent { get; set; }
}
