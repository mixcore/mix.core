using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixdatacontent
{
    public Guid Id { get; set; }

    public int MixDatabaseId { get; set; }

    public string MixDatabaseName { get; set; }

    public Guid? MixDataId { get; set; }

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

    public Guid ParentId { get; set; }

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

    public virtual Mixdatum MixData { get; set; }

    public virtual ICollection<Mixapplication> Mixapplications { get; } = new List<Mixapplication>();

    public virtual ICollection<Mixdatacontentvalue> Mixdatacontentvalues { get; } = new List<Mixdatacontentvalue>();

    public virtual ICollection<Mixmodulecontent> Mixmodulecontents { get; } = new List<Mixmodulecontent>();

    public virtual ICollection<Mixpagecontent> Mixpagecontents { get; } = new List<Mixpagecontent>();

    public virtual ICollection<Mixpostcontent> Mixpostcontents { get; } = new List<Mixpostcontent>();

    public virtual ICollection<Mixtheme> Mixthemes { get; } = new List<Mixtheme>();
}
