using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixTheme : TenantEntityUniqueNameBase<int>
    {
        public string ImageUrl { get; set; }
        public string PreviewUrl { get; set; }
        public string AssetFolder { get; set; }
        public string TemplateFolder { get; set; }
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }
        public MixDataContent MixDataContent { get; set; }
        public virtual ICollection<MixTemplate> MixViewTemplates { get; set; }
    }
}
