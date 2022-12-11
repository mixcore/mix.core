using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixpostcontent
{
    public int Id { get; set; }

    public string ClassName { get; set; }

    public int? MixPostId { get; set; }

    public int? MixPostContentId { get; set; }

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

    public string MixDatabaseName { get; set; }

    public Guid? MixDataContentId { get; set; }

    public virtual ICollection<Mixpostcontent> InverseMixPostContent { get; } = new List<Mixpostcontent>();

    public virtual Mixculture MixCulture { get; set; }

    public virtual Mixdatacontent MixDataContent { get; set; }

    public virtual Mixpost MixPost { get; set; }

    public virtual Mixpostcontent MixPostContent { get; set; }

    public virtual ICollection<Mixpage> Mixpages { get; } = new List<Mixpage>();
}
